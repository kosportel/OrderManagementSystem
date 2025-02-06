using Microsoft.EntityFrameworkCore.Storage;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Infrastructure.DataAccess.DbContexts;

namespace OrderManagement.Infrastructure.Common
{
    public class UnitOfWork(OrderManagementDbContext context,
        IOrderRepository orderRepository,
        IOrderStatusRepository orderStatusRepository,
        IOrderAssignmentRepository orderAssignmentRepository,
        IUserRepository userRepository) : IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        public IOrderRepository OrderRepository => orderRepository;
        public IOrderStatusRepository OrderStatusRepository => orderStatusRepository;
        public IOrderAssignmentRepository OrderAssignmentRepository => orderAssignmentRepository;
        public IUserRepository UserRepository => userRepository;

        public async Task BeginTransactionAsync()
        {
            _transaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
