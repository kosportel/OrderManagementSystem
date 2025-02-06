using OrderManagement.Application.Common;
using OrderManagement.Domain.Common;
using OrderManagement.Domain;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Result<PaginatedResult<Order>> GetOrdersWithFilters(
            int? customerId, List<int>? statusIds, int pageNumber, int pageSize);
        Task<Result<Order>> GetByIdAsync(int id);
        Task<Result<Order>> CreateAsync(Order order);
        Task<Result<bool>> UpdateAsync(Order order);
        Task<Result<bool>> UpdateOrderStatusAsync(int orderId);
        Task<Result<bool>> AssignOrderToDeliveryAsync(int orderId, int deliveryUserId);
        Task<Result<IEnumerable<OrderItem>>> GetOrderItemsAsync(int orderId);
        Task<Result<bool>> DeleteAsync(int id);


    }
}
