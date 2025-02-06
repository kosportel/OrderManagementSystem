using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.DbContexts;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Orders.Persistence
{
    public class OrderStatusRepository(OrderManagementDbContext context, IMapper mapper) : IOrderStatusRepository
    {
        public async Task<Result<IEnumerable<OrderStatus>>> GetByOrderIdAsync(int orderId)
        {
            var statuses = await context.OrderStatuses
                .Where(os => os.OrderId == orderId)
                .ToListAsync();

            return Result<IEnumerable<OrderStatus>>.Success(mapper.Map<IEnumerable<OrderStatus>>(statuses));
        }

        public async Task<Result<bool>> AddStatusAsync(OrderStatus orderStatus)
        {
            var entity = mapper.Map<OrderStatusEntity>(orderStatus);
            context.OrderStatuses.Add(entity);
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
