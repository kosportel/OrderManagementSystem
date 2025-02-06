using OrderManagement.Domain.Enums;

namespace OrderManagement.Contracts.Orders
{
    public record OrderStatusesResponseDto(OrderStatusEnum Enum, string Name);
}
