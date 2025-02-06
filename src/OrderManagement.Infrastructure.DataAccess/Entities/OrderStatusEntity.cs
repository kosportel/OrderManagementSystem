namespace OrderManagement.Infrastructure.DataAccess.Entities
{
    public class OrderStatusEntity
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime DateTimeCreated { get; set; }

        public OrderEntity Order { get; set; } // Navigation Property
    }

}
