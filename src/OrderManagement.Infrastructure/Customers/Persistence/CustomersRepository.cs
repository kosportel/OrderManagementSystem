using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;
using OrderManagement.Infrastructure.DataAccess.DbContexts;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Customers.Persistence;


public class CustomerRepository(OrderManagementDbContext context, IMapper mapper) : ICustomerRepository
{
    public async Task<Result<PaginatedResult<Customer>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = context.Customers
            .Include(c => c.Addresses)
            .Include(c => c.Phones)
            .Include(c => c.User)
            .Where(c => !c.IsDeleted);

        var totalCustomers = await query.CountAsync();
        var customers = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (!customers.Any())
            return Result<PaginatedResult<Customer>>.Failure("No customers found.");

        var domainCustomers = mapper.Map<IEnumerable<Customer>>(customers); 
        var paginatedResult = new PaginatedResult<Customer>(domainCustomers, pageNumber, pageSize, totalCustomers);

        return Result<PaginatedResult<Customer>>.Success(paginatedResult);
    }

    public async Task<Result<Customer>> GetByIdAsync(int id)
    {
        var customerEntity = await context.Customers.Where(x => !x.IsDeleted)
            .Include(x => x.Addresses.Where(y => !y.IsDeleted))
            .Include(x => x.Phones)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        return customerEntity == null
            ? Result<Customer>.Failure("Customer not found.")
            : Result<Customer>.Success(mapper.Map<Customer>(customerEntity));
    }

    public async Task<Result<Customer>> CreateAsync(Customer customer)
    {
        var entity = mapper.Map<CustomerEntity>(customer);
        context.Customers.Add(entity);
        await context.SaveChangesAsync();
        return Result<Customer>.Success(mapper.Map<Customer>(entity));
    }

    public async Task<Result<bool>> UpdateAsync(Customer customer)
    {
        var existingEntity = await context.Customers
            .Include(c => c.Addresses)
            .Include(c => c.Phones)
            .FirstOrDefaultAsync(c => c.Id == customer.Id);

        if (existingEntity == null)
            return Result<bool>.Failure("Customer not found.");

        context.Entry(existingEntity).CurrentValues.SetValues(mapper.Map<CustomerEntity>(customer));

        existingEntity.Addresses.Clear();
        existingEntity.Addresses.AddRange(mapper.Map<IEnumerable<CustomerAddressEntity>>(customer.Addresses));

        existingEntity.Phones.Clear();
        existingEntity.Phones.AddRange(mapper.Map<IEnumerable<CustomerPhoneEntity>>(customer.Phones));

        await context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var customerEntity = await context.Customers.FindAsync(id);
        if (customerEntity == null)
            return Result<bool>.Failure("Customer not found.");

        customerEntity.IsDeleted = true; // Soft delete
        await context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }
}