using AutoMapper;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Orders.Mappers
{
    public class OrderItemEntityProfile : Profile
    {
        public OrderItemEntityProfile()
        {
            // Map OrderItemEntity ↔ OrderItem (Domain Model)
            CreateMap<OrderItemEntity, OrderItem>()
                .ReverseMap();
        }
    }
}
