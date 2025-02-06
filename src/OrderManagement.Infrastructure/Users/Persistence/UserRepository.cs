using AutoMapper;
using OrderManagement.Infrastructure.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.DbContexts;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Common;
using OrderManagement.Application.Interfaces.Repositories;

namespace OrderManagement.Infrastructure.Users.Persistence
{
    public class UserRepository(OrderManagementDbContext context, IMapper mapper) : IUserRepository
    {
        public async Task<Result<PaginatedResult<User>>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = context.Users.Where(m => !m.IsDeleted);
            var totalItems = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!users.Any())
                return Result<PaginatedResult<User>>.Failure("No menu items found.");


            var domainUsers = mapper.Map<IEnumerable<User>>(users);
            var paginatedResult = new PaginatedResult<User>(domainUsers, pageNumber, pageSize, totalItems);

            return Result<PaginatedResult<User>>.Success(paginatedResult);
        }

        public async Task<Result<User>> GetByIdAsync(int id)
        {
            var userEntity = await context.Users.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            return userEntity == null
                ? Result<User>.Failure("User not found.")
                : Result<User>.Success(mapper.Map<User>(userEntity));
        }

        public async Task<Result<User>> GetByEmailAsync(string email)
        {
            var userEntity = await context.Users
                .FirstOrDefaultAsync(m => m.Email == email && !m.IsDeleted);

            return userEntity == null
                ? Result<User>.Failure("User not found.")
                : Result<User>.Success(mapper.Map<User>(userEntity));
        }

        public async Task<Result<User>> CreateAsync(User user)
        {
            var entity = mapper.Map<UserEntity>(user);
            context.Users.Add(entity);
            await context.SaveChangesAsync();
            return Result<User>.Success(mapper.Map<User>(entity));
        }

        public async Task<Result<bool>> UpdateAsync(User user)
        {
            var existingEntity = await context.Users.FindAsync(user.Id);
            if (existingEntity == null || existingEntity.IsDeleted)
                return Result<bool>.Failure("User not found.");

            context.Entry(existingEntity).CurrentValues.SetValues(mapper.Map<UserEntity>(user));
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var userEntity = await context.Users.FindAsync(id);
            if (userEntity == null || userEntity.IsDeleted)
                return Result<bool>.Failure("User not found.");

            userEntity.IsDeleted = true; // Soft delete
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
