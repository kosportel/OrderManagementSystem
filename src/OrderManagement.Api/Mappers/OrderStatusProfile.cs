using AutoMapper;
using OrderManagement.Contracts.Orders;
using OrderManagement.Domain;

namespace OrderManagement.Api.Mappers
{
    public class OrderStatusProfile : Profile
    {
        public OrderStatusProfile()
        {
            CreateMap<OrderStatus, OrderStatusResponseDto>();
            CreateMap<UpdateOrderStatusRequestDto, OrderStatus>();
        }
    }
}
