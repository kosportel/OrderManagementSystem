using OrderManagement.Application.Common;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IMenuItemRepository
    {
        Task<Result<PaginatedResult<MenuItem>>> GetAllAsync(int pageNumber, int pageSize);
        Task<Result<MenuItem>> GetByIdAsync(int id);
        Task<Result<MenuItem>> CreateAsync(MenuItem menuItem);
        Task<Result<bool>> UpdateAsync(MenuItem menuItem);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
