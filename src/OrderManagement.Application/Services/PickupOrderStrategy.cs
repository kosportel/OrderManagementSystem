using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Services
{
    public class PickupOrderStrategy : IOrderStrategy
    {
        public OrderStatusEnum GetFirstStep()
        {
            return OrderStatusEnum.PendingPickup;
        }

        public bool OrderHasStartedPreparation(OrderStatusEnum currentStep)
        {
            return currentStep != GetFirstStep();
        }

        public Result<OrderStatusEnum> GetNextStep(OrderStatusEnum currentStep)
        {
            switch (currentStep)
            {
                case OrderStatusEnum.None:
                    return Result<OrderStatusEnum>.Success(OrderStatusEnum.PendingPickup);

                case OrderStatusEnum.PendingPickup:
                    return Result<OrderStatusEnum>.Success(OrderStatusEnum.PreparingPickup);

                case OrderStatusEnum.PreparingPickup:
                    return Result<OrderStatusEnum>.Success(OrderStatusEnum.ReadyForPickup);

                default:
                    return Result<OrderStatusEnum>.Failure("Invalid or completed pickup order state.");
            }

        }

        public IEnumerable<OrderStatusEnum> GetAvailableStatuses()
        {
            return [OrderStatusEnum.PendingPickup, OrderStatusEnum.PreparingPickup, OrderStatusEnum.ReadyForPickup];
        }
    }
}
