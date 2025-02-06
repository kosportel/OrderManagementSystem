using OrderManagement.Application.Common;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Result<PaginatedResult<Customer>>> GetAllAsync(int pageNumber, int pageSize);
        Task<Result<Customer>> GetByIdAsync(int id);
        Task<Result<Customer>> CreateAsync(Customer customer);
        Task<Result<bool>> UpdateAsync(Customer customer);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
