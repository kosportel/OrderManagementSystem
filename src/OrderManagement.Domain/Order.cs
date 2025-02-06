using OrderManagement.Domain.Enums;
using static OrderManagement.Domain.Customer;

namespace OrderManagement.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public OrderTypeEnum OrderTypeId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime DateTimeCreated { get; set; }

        public Customer Customer { get; set; }
        public CustomerAddress Address { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public List<OrderStatus> OrderStatuses { get; set; } = new();
        public List<OrderAssignment> OrderAssignments { get; set; } = new();
    }
}
