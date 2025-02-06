using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Services
{
    public class OrderStatusService(IOrderStatusRepository orderStatusRepository, IOrderStrategyFactory orderStrategyFactory) : IOrderStatusService
    {
        public async Task<Result<IEnumerable<OrderStatus>>> GetByOrderIdAsync(int orderId)
        {
            return await orderStatusRepository.GetByOrderIdAsync(orderId);
        }

        public async Task<Result<OrderStatus>> GetOrderLatestStatusAsync(int orderId)
        {
            var statuses = await GetByOrderIdAsync(orderId);
            return Result<OrderStatus>.Success(statuses.Value.OrderByDescending(x => x.DateTimeCreated).First());
        }

        public IEnumerable<OrderStatusEnum> GetAvailableStatuses(OrderTypeEnum orderType)
        {
            return orderStrategyFactory.GetStrategy(orderType).GetAvailableStatuses();
        }
    }
}
