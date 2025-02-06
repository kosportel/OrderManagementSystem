using AutoMapper;
using OrderManagement.Infrastructure.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.DbContexts;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Common;
using OrderManagement.Application.Interfaces.Repositories;

namespace OrderManagement.Infrastructure.Menu.Persistence
{
    public class MenuItemRepository(OrderManagementDbContext context, IMapper mapper) : IMenuItemRepository
    {
        public async Task<Result<PaginatedResult<MenuItem>>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = context.MenuItems.Where(m => !m.IsDeleted);
            var totalItems = await query.CountAsync();

            var menuItems = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!menuItems.Any())
                return Result<PaginatedResult<MenuItem>>.Failure("No menu items found.");


            var domainMenuItems = mapper.Map<IEnumerable<MenuItem>>(menuItems);
            var paginatedResult = new PaginatedResult<MenuItem>(domainMenuItems, pageNumber, pageSize, totalItems);

            return Result<PaginatedResult<MenuItem>>.Success(paginatedResult);
        }

        public async Task<Result<MenuItem>> GetByIdAsync(int id)
        {
            var menuItemEntity = await context.MenuItems.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            return menuItemEntity == null
                ? Result<MenuItem>.Failure("Menu item not found.")
                : Result<MenuItem>.Success(mapper.Map<MenuItem>(menuItemEntity));
        }

        public async Task<Result<MenuItem>> CreateAsync(MenuItem menuItem)
        {
            var entity = mapper.Map<MenuItemEntity>(menuItem);
            context.MenuItems.Add(entity);
            await context.SaveChangesAsync();
            return Result<MenuItem>.Success(mapper.Map<MenuItem>(entity));
        }

        public async Task<Result<bool>> UpdateAsync(MenuItem menuItem)
        {
            var existingEntity = await context.MenuItems.FindAsync(menuItem.Id);
            if (existingEntity == null || existingEntity.IsDeleted)
                return Result<bool>.Failure("Menu item not found.");

            context.Entry(existingEntity).CurrentValues.SetValues(mapper.Map<MenuItemEntity>(menuItem));
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var menuItemEntity = await context.MenuItems.FindAsync(id);
            if (menuItemEntity == null || menuItemEntity.IsDeleted)
                return Result<bool>.Failure("Menu item not found.");

            menuItemEntity.IsDeleted = true; // Soft delete
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
