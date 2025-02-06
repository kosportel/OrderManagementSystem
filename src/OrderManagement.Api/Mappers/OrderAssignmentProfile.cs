using AutoMapper;
using OrderManagement.Contracts.Orders;
using OrderManagement.Domain;

namespace OrderManagement.Api.Mappers
{
    public class OrderAssignmentProfile : Profile
    {
        public OrderAssignmentProfile()
        {
            CreateMap<OrderAssignment, OrderAssignmentResponseDto>()
                .ForMember(dest => dest.DeliveryPersonName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.DeliveryPersonEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
        }
    }
}
