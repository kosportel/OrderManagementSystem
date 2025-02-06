using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;

namespace OrderManagement.Application.Services
{

    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        public async Task<Result<PaginatedResult<Customer>>> GetAllAsync(int page, int pageSize)
        {
            return await customerRepository.GetAllAsync(page, pageSize);
        }

        public async Task<Result<Customer>> GetByIdAsync(int id)
        {
            return await customerRepository.GetByIdAsync(id);
        }

        public async Task<Result<Customer>> CreateAsync(Customer customer)
        {
            return await customerRepository.CreateAsync(customer);
        }

        public async Task<Result<bool>> UpdateAsync(Customer customer)
        {
            return await customerRepository.UpdateAsync(customer);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await customerRepository.DeleteAsync(id);
        }
    }
}
