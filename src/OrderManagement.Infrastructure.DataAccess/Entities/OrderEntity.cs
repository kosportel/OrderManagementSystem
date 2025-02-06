namespace OrderManagement.Infrastructure.DataAccess.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public int OrderTypeId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime DateTimeCreated { get; set; }
        public bool IsDeleted { get; set; } = false;

        public CustomerEntity Customer { get; set; }
        public CustomerAddressEntity Address { get; set; }

        public List<OrderItemEntity> OrderItems { get; set; } = [];
        public List<OrderStatusEntity> OrderStatuses { get; set; } = [];
        public List<OrderAssignmentEntity> OrderAssignments { get; set; } = [];
    }

}
