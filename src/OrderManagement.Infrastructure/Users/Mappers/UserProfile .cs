using AutoMapper;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Users.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
