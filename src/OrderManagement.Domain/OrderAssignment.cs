namespace OrderManagement.Domain
{
    public class OrderAssignment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsCompleted { get; set; } 

        public Order Order { get; set; }
        public User User { get; set; }
    }
}
