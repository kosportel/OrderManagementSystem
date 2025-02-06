using OrderManagement.Application.Common;
using OrderManagement.Domain;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderStatusService
    {
        Task<Result<IEnumerable<OrderStatus>>> GetByOrderIdAsync(int orderId);
        Task<Result<OrderStatus>> GetOrderLatestStatusAsync(int orderId);
        IEnumerable<OrderStatusEnum> GetAvailableStatuses(OrderTypeEnum orderType);
    }
}
