using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Services
{
    public class OrderStrategyFactory : IOrderStrategyFactory
    {
        private readonly Dictionary<OrderTypeEnum, IOrderStrategy> _strategies = new()
        {
            { OrderTypeEnum.Pickup, new PickupOrderStrategy() },
            { OrderTypeEnum.Delivery, new DeliveryOrderStrategy() }
        };

        public IOrderStrategy GetStrategy(OrderTypeEnum orderType)
        {
            if (_strategies.TryGetValue(orderType, out var strategy))
            {
                return strategy;
            }
            throw new ArgumentException("Invalid order type", nameof(orderType));
        }
    }
}
