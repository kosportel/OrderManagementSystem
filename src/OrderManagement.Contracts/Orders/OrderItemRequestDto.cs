namespace OrderManagement.Contracts.Orders
{
    public class OrderItemRequestDto
    {
        public required int MenuItemId { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
