using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Common;
using OrderManagement.Domain;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Services
{
    public class OrderService(IUnitOfWork unitOfWork, IOrderStatusService orderStatusService, IOrderStrategyFactory orderStrategyFactory) : IOrderService
    {
        public Result<PaginatedResult<Order>> GetOrdersWithFilters(
            int? customerId, List<int>? statusIds, int pageNumber, int pageSize)
        {
            var query = unitOfWork.OrderRepository.GetQueryable();

            // Filter by CustomerId if provided
            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            // Filter by latest order status
            if (statusIds != null && statusIds.Any())
            {
                query = query.Where(o => o.OrderStatuses
                                             .OrderByDescending(os => os.DateTimeCreated)
                                             .FirstOrDefault() != null &&
                                         statusIds.Contains((int)o.OrderStatuses
                                             .OrderByDescending(os => os.DateTimeCreated)
                                             .FirstOrDefault().OrderStatusId));
            }

            var totalItems = query.Count();

            var orders = query
                .OrderBy(o => o.DateTimeCreated) // Optional: Order latest orders first
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!orders.Any())
                return Result<PaginatedResult<Order>>.Failure("No orders found.");

            var paginatedResult = new PaginatedResult<Order>(orders, pageNumber, pageSize, totalItems);

            return Result<PaginatedResult<Order>>.Success(paginatedResult);
        }

        public async Task<Result<Order>> GetByIdAsync(int id)
        {
            return await unitOfWork.OrderRepository.GetByIdAsync(id);
        }

        public async Task<Result<Order>> CreateAsync(Order order)
        {
            // Validate business rules
            if (order.OrderItems == null || !order.OrderItems.Any())
                return Result<Order>.Failure("An order must contain at least one item.");

            var orderStepResult = orderStrategyFactory.GetStrategy(order.OrderTypeId).GetNextStep(OrderStatusEnum.None);
            if (!orderStepResult.IsSuccess)
                return Result<Order>.Failure($"Failed to find the order step for the type {order.OrderTypeId}.");

            try
            {
                await unitOfWork.BeginTransactionAsync();

                var createdOrder = await unitOfWork.OrderRepository.CreateAsync(order);
                if (!createdOrder.IsSuccess)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return Result<Order>.Failure("Failed to create order.");
                }

                var initialStatus = new OrderStatus
                {
                    OrderId = createdOrder.Value.Id,
                    OrderStatusId = orderStepResult.Value,
                    DateTimeCreated = DateTime.UtcNow
                };
                var statusResult = await unitOfWork.OrderStatusRepository.AddStatusAsync(initialStatus);
                if (!statusResult.IsSuccess)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return Result<Order>.Failure("Failed to set initial order status.");
                }

                await unitOfWork.CommitTransactionAsync();

                return await unitOfWork.OrderRepository.GetByIdAsync(createdOrder.Value.Id);

            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<Order>.Failure($"Transaction failed: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateAsync(Order order)
        {
            var orderHasStarted = await OrderHasStartedPreparation(order.Id);
            if (!orderHasStarted.IsSuccess)
                return orderHasStarted;

            return await unitOfWork.OrderRepository.UpdateAsync(order);
        }

        public async Task<Result<bool>> UpdateOrderStatusAsync(int orderId)
        {
            var order = await unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (!order.IsSuccess)
                return Result<bool>.Failure(order.Error ?? "Order not found.");

            var latestStatus= await orderStatusService.GetOrderLatestStatusAsync(orderId);
            if (!latestStatus.IsSuccess)
                return Result<bool>.Failure(latestStatus.Error);

            var transitionStatus = orderStrategyFactory.GetStrategy(order.Value.OrderTypeId).GetNextStep(latestStatus.Value.OrderStatusId);
            if (!transitionStatus.IsSuccess)
                return Result<bool>.Failure("Invalid order status transition.");

            // Add new status record
            var newStatus = new OrderStatus
            {
                OrderId = orderId,
                OrderStatusId = transitionStatus.Value,
                DateTimeCreated = DateTime.UtcNow
            };

            var statusUpdateResult = await unitOfWork.OrderStatusRepository.AddStatusAsync(newStatus);
            if (!statusUpdateResult.IsSuccess)
                return Result<bool>.Failure("Failed to update order status.");

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> AssignOrderToDeliveryAsync(int orderId, int deliveryUserId)
        {
            try
            {
                var latestStatus = await orderStatusService.GetOrderLatestStatusAsync(orderId);

                // Ensure order is "Ready for Delivery" before assigning
                if (latestStatus.Value.OrderStatusId != OrderStatusEnum.ReadyForDelivery)
                    return Result<bool>.Failure("Order is not ready for delivery.");

                // Check if delivery user exists and is available
                var deliveryUserResult = await unitOfWork.UserRepository.GetByIdAsync(deliveryUserId);
                if (!deliveryUserResult.IsSuccess || deliveryUserResult.Value.RoleId != UserRoleEnum.Delivery)
                    return Result<bool>.Failure("Invalid delivery staff.");

                var orderAssignment = new OrderAssignment
                {
                    OrderId = orderId,
                    UserId = deliveryUserId,
                    CreatedDateTime = DateTime.UtcNow,
                    IsCompleted = true
                };

                var assignResult = await unitOfWork.OrderAssignmentRepository.CreateAsync(orderAssignment);
                if (!assignResult.IsSuccess)
                    return Result<bool>.Failure("Failed to assign order to delivery staff.");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to assign the order to delivery: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<OrderItem>>> GetOrderItemsAsync(int orderId)
        {
            return await unitOfWork.OrderRepository.GetOrderItemsAsync(orderId);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var orderHasStarted = await OrderHasStartedPreparation(id);
            if (!orderHasStarted.IsSuccess)
                return orderHasStarted;

            var deleteResult = await unitOfWork.OrderRepository.DeleteAsync(id);
            return !deleteResult.IsSuccess ? Result<bool>.Failure("Failed to cancel order.") : Result<bool>.Success(true);
        }

        private async Task<Result<bool>> OrderHasStartedPreparation(int id)
        {
            // Fetch the order and its latest status
            var orderResult = await unitOfWork.OrderRepository.GetByIdAsync(id);
            if (!orderResult.IsSuccess)
                return Result<bool>.Failure("Order not found.");

            var order = orderResult.Value;

            // Get the latest status of the order
            var latestStatusResult = await orderStatusService.GetOrderLatestStatusAsync(id);
            if (!latestStatusResult.IsSuccess)
                return Result<bool>.Failure("Unable to retrieve order status.");

            var orderFirstStep = orderStrategyFactory.GetStrategy(order.OrderTypeId).GetFirstStep();
            
            // Check if the order is still in a cancellable state
            if (latestStatusResult.Value.OrderStatusId != orderFirstStep)
                return Result<bool>.Failure("Order cannot altered/canceled because it has already started preparation.");

            return Result<bool>.Success(true);
        }
    }
}
