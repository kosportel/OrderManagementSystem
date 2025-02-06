using AutoMapper;
using OrderManagement.Contracts.Customers;
using OrderManagement.Contracts.Orders;
using OrderManagement.Domain;
using static OrderManagement.Domain.Customer;

namespace OrderManagement.Api.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResponseCompactDto>();

            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.OrderStatuses, opt => opt.MapFrom(src => src.OrderStatuses))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForPath(dest => dest.Customer.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<Customer, CustomerResponseSimpleDto>();

            CreateMap<CustomerAddress, CustomerResponseDto.CustomerAddressResponseDto>();
            CreateMap<CustomerAddress, CustomerResponseSimpleDto.CustomerAddressResponseDto>();

            CreateMap<OrderRequestDto, Order>();
        }
    }
}
