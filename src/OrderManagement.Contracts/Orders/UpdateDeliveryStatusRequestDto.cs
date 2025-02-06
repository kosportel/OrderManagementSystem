namespace OrderManagement.Contracts.Orders
{
    public class UpdateDeliveryStatusDto
    {
        public int UserId { get; set; }
        public bool IsDelivered { get; set; }
        public string? Notes { get; set; }
    }
}
