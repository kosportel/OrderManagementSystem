using AutoMapper;
using OrderManagement.Contracts.Customers;
using OrderManagement.Domain;

namespace OrderManagement.Api.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerRequestDto, Customer>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses))
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));
            CreateMap<CustomerRequestDto.CustomerAddressDto, Customer.CustomerAddress>();
            CreateMap<CustomerRequestDto.CustomerPhoneDto, Customer.CustomerPhone>();


            // Mapping Customer -> CustomerResponseDto
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses))
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));

            // Mapping CustomerAddress -> CustomerAddressResponseDto
            CreateMap<Customer.CustomerAddress, CustomerResponseDto.CustomerAddressResponseDto>();

            // Mapping CustomerPhone -> CustomerPhoneResponseDto
            CreateMap<Customer.CustomerPhone, CustomerResponseDto.CustomerPhoneResponseDto>();
        }
    }
}
