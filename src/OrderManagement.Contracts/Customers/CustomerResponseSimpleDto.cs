using OrderManagement.Contracts.Users;

namespace OrderManagement.Contracts.Customers
{
    public class CustomerResponseSimpleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public UserResponseDto User { get; set; }

        public CustomerAddressResponseDto Address { get; set; }

        public class CustomerAddressResponseDto
        {
            public int Id { get; set; }
            public string Street { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string PostalCode { get; set; } = string.Empty;
            public string BuildingNr { get; set; } = string.Empty;
            public int Floor { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
