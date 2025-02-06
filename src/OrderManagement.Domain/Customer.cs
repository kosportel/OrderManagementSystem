namespace OrderManagement.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public bool IsDeleted { get; set; } = false;

        public User User { get; set; }
        public IEnumerable<CustomerAddress> Addresses { get; set; } = [];
        public IEnumerable<CustomerPhone> Phones { get; set; } = [];

        public void MarkForDelete()
        {
            IsDeleted = true;
        }

        public class CustomerAddress
        {
            public int Id { get; set; }
            public string Street { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string PostalCode { get; set; } = string.Empty;
            public string BuildingNr { get; set; } = string.Empty;
            public int Floor { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public bool IsDeleted { get; set; } = false;
        }

        public class CustomerPhone
        {
            public int Id { get; set; }
            public string Telephone { get; set; } = string.Empty;
        }

    }
}
