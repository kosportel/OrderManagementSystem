using AutoMapper;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Orders.Mappers
{
    public class OrderAssignmentProfile : Profile
    {
        public OrderAssignmentProfile()
        {
            // ✅ Map OrderAssignmentEntity (DB) -> OrderAssignment (Domain)
            CreateMap<OrderAssignmentEntity, OrderAssignment>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            // ✅ Map OrderAssignment (Domain) -> OrderAssignmentEntity (DB)
            CreateMap<OrderAssignment, OrderAssignmentEntity>()
                .ForMember(dest => dest.Order, opt => opt.Ignore()) // Avoid circular references
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Avoid circular references
        }
    }
}
