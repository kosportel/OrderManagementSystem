using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Infrastructure.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.Entities;
using AutoMapper;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Infrastructure.Orders.Persistence
{
    public class OrderAssignmentRepository(OrderManagementDbContext context, IMapper mapper) : IOrderAssignmentRepository
    {
        public async Task<Result<OrderAssignment>> GetByOrderIdAsync(int orderId)
        {
            var assignment = await context.OrderAssignments
                .Include(a => a.Order)
                .FirstOrDefaultAsync(a => a.OrderId == orderId && !a.IsCompleted);

            return assignment == null
                ? Result<OrderAssignment>.Failure("No active assignment found.")
                : Result<OrderAssignment>.Success(mapper.Map<OrderAssignment>(assignment));
        }

        public async Task<Result<IEnumerable<OrderAssignment>>> GetActiveAssignmentsByUserIdAsync(int userId)
        {
            var assignments = await context.OrderAssignments
                .Include(x => x.User)
                .Include(x => x.Order)
                .ThenInclude(x => x.Customer)
                .Include(x => x.Order)
                .ThenInclude(x => x.Address)
                .Where(a => a.UserId == userId && !a.IsCompleted)
                .ToListAsync();

            return assignments.Any()
                ? Result<IEnumerable<OrderAssignment>>.Success(mapper.Map<IEnumerable<OrderAssignment>>(assignments))
                : Result<IEnumerable<OrderAssignment>>.Failure("No active assignments found.");
        }

        public async Task<Result<IEnumerable<OrderAssignment>>> GetAllActiveAssignmentsAsync()
        {
            var activeAssignments = await context.OrderAssignments
                .Include(x => x.User)
                .Include(x => x.Order)
                .ThenInclude(x => x.Customer)
                .Include(x => x.Order)
                .ThenInclude(x => x.Address)
                .Where(oa => !oa.IsCompleted)
                .ToListAsync();

            return Result<IEnumerable<OrderAssignment>>.Success(mapper.Map<IEnumerable<OrderAssignment>>(activeAssignments));
        }

        public async Task<Result<IEnumerable<User>>> GetIdleUsersAsync()
        {
            var idleDeliveryUsers = await context.Users
                .Where(x => x.RoleId == (int)UserRoleEnum.Delivery)
                .GroupJoin(
                    context.OrderAssignments.Where(oa => !oa.IsCompleted), // Filter active assignments
                    user => user.Id,
                    assignment => assignment.UserId,
                    (user, assignments) => new { user, assignments }
                )
                .Where(result => !result.assignments.Any()) // Users with no active assignments
                .Select(result => result)
                .ToListAsync();

            return Result<IEnumerable<User>>.Success(mapper.Map<IEnumerable<User>>(idleDeliveryUsers.Select(x => x.user)));
        }

        public async Task<Result<OrderAssignment>> CreateAsync(OrderAssignment orderAssignment)
        {
            try
            {
                var entity = new OrderAssignmentEntity
                {
                    OrderId = orderAssignment.OrderId,
                    UserId = orderAssignment.UserId,
                    CreatedDateTime = DateTime.UtcNow,
                    IsCompleted = false
                };

                await context.OrderAssignments.AddAsync(entity);
                await context.SaveChangesAsync();

                var createdAssignment = new OrderAssignment
                {
                    Id = entity.Id,
                    OrderId = entity.OrderId,
                    UserId = entity.UserId,
                    CreatedDateTime = entity.CreatedDateTime,
                    IsCompleted = entity.IsCompleted
                };

                return Result<OrderAssignment>.Success(createdAssignment);
            }
            catch (Exception ex)
            {
                return Result<OrderAssignment>.Failure($"Failed to create order assignment: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateAsync(OrderAssignment assignment)
        {
            var existingAssignment = await context.OrderAssignments.FindAsync(assignment.Id);
            if (existingAssignment == null)
                return Result<bool>.Failure("Assignment not found.");

            context.Entry(existingAssignment).CurrentValues.SetValues(mapper.Map<OrderAssignmentEntity>(assignment));
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
