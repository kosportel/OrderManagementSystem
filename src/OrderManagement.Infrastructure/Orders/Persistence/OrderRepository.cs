using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;
using OrderManagement.Infrastructure.DataAccess.DbContexts;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Orders.Persistence
{
    public class OrderRepository(OrderManagementDbContext context, IMapper mapper) : IOrderRepository
    {
        public async Task<Result<PaginatedResult<Order>>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = context.Orders;
            var totalItems = await query.CountAsync();

            var orders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!orders.Any())
                return Result<PaginatedResult<Order>>.Failure("No orders found.");

            var domainOrders = mapper.Map<IEnumerable<Order>>(orders);
            var paginatedResult = new PaginatedResult<Order>(domainOrders, pageNumber, pageSize, totalItems);

            return Result<PaginatedResult<Order>>.Success(paginatedResult);
        }

        public async Task<Result<Order>> GetByIdAsync(int id)
        {
            var orderEntity = await context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderStatuses)
                .Include(o => o.Customer)
                .Include(o => o.Address)
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(o => o.Id == id);

            return orderEntity == null
                ? Result<Order>.Failure("Order not found.")
                : Result<Order>.Success(mapper.Map<Order>(orderEntity));
        }

        public async Task<Result<Order>> CreateAsync(Order order)
        {
            var entity = mapper.Map<OrderEntity>(order);
            context.Orders.Add(entity);
            await context.SaveChangesAsync();
            return Result<Order>.Success(mapper.Map<Order>(entity));
        }

        public async Task<Result<bool>> UpdateAsync(Order order)
        {
            var existingEntity = await context.Orders.FindAsync(order.Id);
            if (existingEntity == null || existingEntity.IsDeleted)
                return Result<bool>.Failure("Order not found.");

            context.Entry(existingEntity).CurrentValues.SetValues(mapper.Map<OrderEntity>(order));

            existingEntity.OrderItems.Clear();
            existingEntity.OrderItems.AddRange(mapper.Map<IEnumerable<OrderItemEntity>>(order.OrderItems));

            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var orderEntity = await context.Orders.FindAsync(id);
            if (orderEntity == null || orderEntity.IsDeleted)
                return Result<bool>.Failure("Order not found.");

            orderEntity.IsDeleted = true; // Soft delete
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<OrderItem>>> GetOrderItemsAsync(int orderId)
        {
            var orderItems = await context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();

            return Result<IEnumerable<OrderItem>>.Success(mapper.Map<IEnumerable<OrderItem>>(orderItems));
        }

        public IQueryable<Order> GetQueryable()
        {
            return context.Orders
                .Include(o => o.OrderStatuses)
                .Where(x => !x.IsDeleted)
                .AsEnumerable()  
                .Select(mapper.Map<Order>)
                .AsQueryable();
        }
    }
}
