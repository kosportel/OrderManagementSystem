using OrderManagement.Application.Common;
using OrderManagement.Domain;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IOrderStatusRepository
    {
        Task<Result<IEnumerable<OrderStatus>>> GetByOrderIdAsync(int orderId);
        Task<Result<bool>> AddStatusAsync(OrderStatus orderStatus);
    }
}
