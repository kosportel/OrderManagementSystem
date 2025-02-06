using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Contracts.MenuItem
{
    public class MenuItemRequestDto
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Ingredients { get; set; } = string.Empty;
        [Required] public string Allergies { get; set; } = string.Empty;
        [Required] public decimal Price { get; set; }
        [Required] public int ExpectedPrepMinutes { get; set; }
    }
}
