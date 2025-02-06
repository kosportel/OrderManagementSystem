namespace OrderManagement.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }

}
