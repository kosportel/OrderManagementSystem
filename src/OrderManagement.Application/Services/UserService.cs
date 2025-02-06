using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;

namespace OrderManagement.Application.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<Result<PaginatedResult<User>>> GetAllAsync(int page, int pageSize)
        {
            return await userRepository.GetAllAsync(page, pageSize);
        }

        public async Task<Result<User>> GetByIdAsync(int id)
        {
            return await userRepository.GetByIdAsync(id);
        }

        public async Task<Result<User>> GetByEmailAsync(string email)
        {
            return await userRepository.GetByEmailAsync(email);
        }

        public async Task<Result<User>> CreateAsync(User user)
        {
            return await userRepository.CreateAsync(user);
        }

        public async Task<Result<bool>> UpdateAsync(User user)
        {
            return await userRepository.UpdateAsync(user);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await userRepository.DeleteAsync(id);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await userRepository.GetByEmailAsync(email);
            if (user == null || !PasswordHasher.VerifyPassword(password, user.Value.PasswordHash))
                return null; // Invalid credentials

            return user.Value; // Valid user
        }
    }
}
