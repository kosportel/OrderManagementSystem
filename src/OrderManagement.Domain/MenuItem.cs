namespace OrderManagement.Domain
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int ExpectedPrepMinutes { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
