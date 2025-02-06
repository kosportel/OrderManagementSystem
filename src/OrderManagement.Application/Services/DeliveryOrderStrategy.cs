using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Services
{
    public class DeliveryOrderStrategy : IOrderStrategy
    {
        public OrderStatusEnum GetFirstStep()
        {
            return OrderStatusEnum.PendingDelivery;
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
                    return Result<OrderStatusEnum>.Success(OrderStatusEnum.PendingDelivery);

                case OrderStatusEnum.PendingDelivery:
                    return Result<OrderStatusEnum>.Success(OrderStatusEnum.PreparingDelivery);

                case OrderStatusEnum.PreparingDelivery:
                    return Result<OrderStatusEnum>.Success(OrderStatusEnum.ReadyForDelivery);

                default:
                    return Result<OrderStatusEnum>.Failure("Invalid or completed delivery order state.");
            }
        }

        public IEnumerable<OrderStatusEnum> GetAvailableStatuses()
        {
            return [OrderStatusEnum.PendingDelivery, OrderStatusEnum.PreparingDelivery, OrderStatusEnum.ReadyForDelivery];
        }
    }
}
