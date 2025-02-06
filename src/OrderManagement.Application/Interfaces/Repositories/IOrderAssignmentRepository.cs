using OrderManagement.Application.Common;
using OrderManagement.Domain;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IOrderAssignmentRepository
    {
        Task<Result<OrderAssignment>> GetByOrderIdAsync(int orderId);
        Task<Result<IEnumerable<OrderAssignment>>> GetActiveAssignmentsByUserIdAsync(int userId);

        Task<Result<IEnumerable<OrderAssignment>>> GetAllActiveAssignmentsAsync();
        Task<Result<IEnumerable<User>>> GetIdleUsersAsync();

        Task<Result<OrderAssignment>> CreateAsync(OrderAssignment orderAssignment);
        Task<Result<bool>> UpdateAsync(OrderAssignment assignment);
    }
}
