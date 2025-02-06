namespace OrderManagement.Infrastructure.DataAccess.Entities;

public class CustomerEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public bool IsDeleted { get; set; } = false;

    public UserEntity User { get; set; }
    public List<CustomerAddressEntity> Addresses { get; set; } = [];
    public List<CustomerPhoneEntity> Phones { get; set; } = [];
}
