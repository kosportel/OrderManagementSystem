namespace OrderManagement.Infrastructure.DataAccess.Entities
{
    public class OrderAssignmentEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsCompleted { get; set; } = false;

        public OrderEntity Order { get; set; }
        public UserEntity User { get; set; }
    }
}
