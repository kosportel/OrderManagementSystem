﻿using OrderManagement.Application.Common;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<Result<PaginatedResult<User>>> GetAllAsync(int pageNumber, int pageSize);
        Task<Result<User>> GetByIdAsync(int id);
        Task<Result<User>> GetByEmailAsync(string email);
        Task<Result<User>> CreateAsync(User user);
        Task<Result<bool>> UpdateAsync(User user);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
