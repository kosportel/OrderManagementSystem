using OrderManagement.Application.Common;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Result<PaginatedResult<Order>>> GetAllAsync(int pageNumber, int pageSize);
        Task<Result<Order>> GetByIdAsync(int id);
        Task<Result<Order>> CreateAsync(Order order);
        Task<Result<bool>> UpdateAsync(Order order);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<IEnumerable<OrderItem>>> GetOrderItemsAsync(int orderId);
        IQueryable<Order> GetQueryable();
    }
}
