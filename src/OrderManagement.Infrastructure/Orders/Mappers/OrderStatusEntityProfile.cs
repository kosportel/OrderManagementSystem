using AutoMapper;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Orders.Mappers
{
    public class OrderStatusEntityProfile : Profile
    {
        public OrderStatusEntityProfile()
        {
            // Map OrderStatusEntity ↔ OrderStatuses (Domain Model)
            CreateMap<OrderStatusEntity, OrderStatus>()
                .ReverseMap();
        }
    }
}
