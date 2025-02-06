using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Contracts.Customers
{
    public class CustomerRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        
        [Required] public IEnumerable<CustomerAddressDto> Addresses { get; set; } = [];
        [Required] public IEnumerable<CustomerPhoneDto> Phones { get; set; } = [];

        public class CustomerAddressDto
        {
            public int Id { get; set; }
            [Required] public string Street { get; set; } = string.Empty;
            [Required] public string City { get; set; } = string.Empty;
            [Required] public string PostalCode { get; set; } = string.Empty;
            [Required] public string BuildingNr { get; set; } = string.Empty;
            [Required] public int Floor { get; set; }
            [Required] public double Latitude { get; set; }
            [Required] public double Longitude { get; set; }
            [Required] public bool IsDeleted { get; set; } = false;
        }

        public class CustomerPhoneDto
        {
            public int Id { get; set; }
            [Required] public string Telephone { get; set; } = string.Empty;
        }

    }
}
