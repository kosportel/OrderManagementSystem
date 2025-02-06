using AutoMapper;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Customers.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Domain - Entity
            CreateMap<Customer, CustomerEntity>().ReverseMap();
            CreateMap<Customer.CustomerAddress, CustomerAddressEntity>().ReverseMap();
            CreateMap<Customer.CustomerPhone, CustomerPhoneEntity>().ReverseMap();
        }
    }
}
