using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Services
{
    public class DeliveryService(IUnitOfWork unitOfWork) : IDeliveryService
    {
        public async Task<Result<IEnumerable<OrderAssignment>>> GetAllAssignedDeliveriesAsync()
        {
            var result = await unitOfWork.OrderAssignmentRepository.GetAllActiveAssignmentsAsync();
            return result.IsSuccess ? Result<IEnumerable<OrderAssignment>>.Success(result.Value)
                : Result<IEnumerable<OrderAssignment>>.Failure(result.Error);
        }

        public async Task<Result<IEnumerable<User>>> GetIdleDeliveryPeopleAsync()
        {
            var idleUsers = await unitOfWork.OrderAssignmentRepository.GetIdleUsersAsync();
            if (!idleUsers.IsSuccess || !idleUsers.Value.Any())
                return Result<IEnumerable<User>>.Failure("No idle delivery staff found.");

            return Result<IEnumerable<User>>.Success(idleUsers.Value);
        }

        public async Task<Result<IEnumerable<Order>>> GetAssignedDeliveriesAsync(int deliveryUserId)
        {
            // Fetch orders assigned to this delivery user
            var assignedOrdersResult = await unitOfWork.OrderAssignmentRepository.GetActiveAssignmentsByUserIdAsync(deliveryUserId);
            if (!assignedOrdersResult.IsSuccess || !assignedOrdersResult.Value.Any())
                return Result<IEnumerable<Order>>.Failure("No assigned deliveries found.");

            var assignedOrders = assignedOrdersResult.Value.Select(a => a.Order);
            return Result<IEnumerable<Order>>.Success(assignedOrders);
        }

        public async Task<Result<bool>> StartDeliveryAsync(int orderId, int deliveryUserId)
        {
            // Update order status
            var newStatus = new OrderStatus
            {
                OrderId = orderId,
                OrderStatusId = OrderStatusEnum.OutForDelivery,
                DateTimeCreated = DateTime.UtcNow
            };

            var statusUpdateResult = await unitOfWork.OrderStatusRepository.AddStatusAsync(newStatus);
            return !statusUpdateResult.IsSuccess ? Result<bool>.Failure("Failed to update order status.") : Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateDeliveryStatusAsync(int orderId, int deliveryUserId, bool isDelivered)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync(); // Start transaction

                // Fetch the assigned delivery
                var assignmentResult = await unitOfWork.OrderAssignmentRepository.GetByOrderIdAsync(orderId);
                if (!assignmentResult.IsSuccess || assignmentResult.Value.UserId != deliveryUserId)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return Result<bool>.Failure("Order is not assigned to this delivery staff.");
                }

                // Update order status
                var newStatus = new OrderStatus
                {
                    OrderId = orderId,
                    OrderStatusId = isDelivered ? OrderStatusEnum.Delivered : OrderStatusEnum.UnableToDeliver,
                    DateTimeCreated = DateTime.UtcNow
                };

                var statusUpdateResult = await unitOfWork.OrderStatusRepository.AddStatusAsync(newStatus);
                if (!statusUpdateResult.IsSuccess)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return Result<bool>.Failure("Failed to update order status.");
                }

                // Mark assignment as completed
                var assignment = assignmentResult.Value;
                assignment.IsCompleted = true;
                await unitOfWork.OrderAssignmentRepository.UpdateAsync(assignment);

                await unitOfWork.CommitTransactionAsync(); // Commit transaction
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<bool>.Failure($"Transaction failed: {ex.Message}");
            }
        }
    }
}
