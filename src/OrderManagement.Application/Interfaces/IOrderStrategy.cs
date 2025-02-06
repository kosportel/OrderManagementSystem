using OrderManagement.Application.Common;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderStrategy
    {
        OrderStatusEnum GetFirstStep();

        bool OrderHasStartedPreparation(OrderStatusEnum currentStep);

        Result<OrderStatusEnum> GetNextStep(OrderStatusEnum currentStep);

        IEnumerable<OrderStatusEnum> GetAvailableStatuses();
    }
}
