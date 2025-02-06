using AutoMapper;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Orders.Mappers
{
    public class OrderEntityProfile : Profile
    {
        public OrderEntityProfile()
        {
            // Map OrderEntity ↔ Order (Domain Model)
            CreateMap<OrderEntity, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.OrderStatuses, opt => opt.MapFrom(src => src.OrderStatuses))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.OrderAssignments, opt => opt.MapFrom(src => src.OrderAssignments))
                .ReverseMap();
        }
    }
}
