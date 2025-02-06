namespace OrderManagement.Infrastructure.DataAccess.Entities
{
    public class OrderItemEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;

        public OrderEntity Order { get; set; } // Navigation Property
        public MenuItemEntity MenuItem { get; set; } // Navigation Property
    }
}
