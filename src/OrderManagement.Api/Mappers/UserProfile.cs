using AutoMapper;
using OrderManagement.Contracts.Customers;
using OrderManagement.Contracts.Users;
using OrderManagement.Domain;

namespace OrderManagement.Api.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequestDto, User>().ReverseMap().ForMember(x => x.Password, m => m.MapFrom(u => u.PasswordHash));
            CreateMap<UserResponseDto, User>().ReverseMap(); 
        }
    }
}
