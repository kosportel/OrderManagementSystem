namespace OrderManagement.Contracts.Orders
{
    public class OrderAssignmentResponseDto
    {
        public int Id { get; set; } 
        public int OrderId { get; set; } 
        public int UserId { get; set; } 

        public string DeliveryPersonName { get; set; } = string.Empty; 
        public string DeliveryPersonEmail { get; set; } = string.Empty; 

        public DateTime CreatedDateTime { get; set; } 
        public bool IsCompleted { get; set; }

        public OrderResponseDto Order { get; set; } 
    }
}
