using AutoMapper;
using OrderManagement.Contracts.Orders;
using OrderManagement.Domain;

namespace OrderManagement.Api.Mappers
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemResponseDto>();
            CreateMap<OrderItemRequestDto, OrderItem>();
        }
    }
}
