using AutoMapper;
using OrderManagement.Domain;
using OrderManagement.Infrastructure.DataAccess.Entities;

namespace OrderManagement.Infrastructure.Menu.Mappers
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItemEntity, MenuItem>().ReverseMap();
        }
    }
}
