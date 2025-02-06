using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderStrategyFactory
    {
        IOrderStrategy GetStrategy(OrderTypeEnum orderType);
    }
}
