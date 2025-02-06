namespace OrderManagement.Infrastructure.DataAccess.Entities
{
    public class MenuItemEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Ingredients { get; set; }
        public required string Allergies { get; set; }
        public decimal Price { get; set; }
        public int ExpectedPrepMinutes { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
