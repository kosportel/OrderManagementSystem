using OrderManagement.Application.Common;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;

namespace OrderManagement.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<PaginatedResult<Customer>>> GetAllAsync(int page, int pageSize);
        Task<Result<Customer>> GetByIdAsync(int id);
        Task<Result<Customer>> CreateAsync(Customer customer);
        Task<Result<bool>> UpdateAsync(Customer customer);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
