using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain
{
    public class OrderStatus
    {
        public int OrderId { get; set; }
        public OrderStatusEnum OrderStatusId { get; set; }
        public DateTime DateTimeCreated { get; set; }

        public Order Order { get; set; }
    }

}
