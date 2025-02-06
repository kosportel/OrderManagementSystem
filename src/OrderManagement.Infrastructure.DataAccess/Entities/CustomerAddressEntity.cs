namespace OrderManagement.Infrastructure.DataAccess.Entities
{
    public class CustomerAddressEntity
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
}