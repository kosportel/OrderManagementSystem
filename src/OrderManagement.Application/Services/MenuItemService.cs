using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;

namespace OrderManagement.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<Result<PaginatedResult<MenuItem>>> GetAllAsync(int page, int pageSize)
        {
            return await _menuItemRepository.GetAllAsync(page, pageSize);
        }

        public async Task<Result<MenuItem>> GetByIdAsync(int id)
        {
            return await _menuItemRepository.GetByIdAsync(id);
        }

        public async Task<Result<MenuItem>> CreateAsync(MenuItem menuItem)
        {
            return await _menuItemRepository.CreateAsync(menuItem);
        }

        public async Task<Result<bool>> UpdateAsync(MenuItem menuItem)
        {
            return await _menuItemRepository.UpdateAsync(menuItem);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _menuItemRepository.DeleteAsync(id);
        }
    }
}
