namespace OrderManagement.Contracts.Orders
{
    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
