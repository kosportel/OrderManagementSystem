using OrderManagement.Application.Common;
using OrderManagement.Domain;

namespace OrderManagement.Application.Interfaces
{
    public interface IDeliveryService
    {
        Task<Result<IEnumerable<OrderAssignment>>> GetAllAssignedDeliveriesAsync();
        Task<Result<IEnumerable<User>>> GetIdleDeliveryPeopleAsync();


        Task<Result<IEnumerable<Order>>> GetAssignedDeliveriesAsync(int deliveryUserId);
        Task<Result<bool>> UpdateDeliveryStatusAsync(int orderId, int deliveryUserId, bool isDelivered);
        Task<Result<bool>> StartDeliveryAsync(int orderId, int deliveryUserId);
    }
}
