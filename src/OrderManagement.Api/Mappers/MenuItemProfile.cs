using AutoMapper;
using OrderManagement.Contracts.MenuItem;
using OrderManagement.Domain;

namespace OrderManagement.Api.Mappers
{
    public class MenuItemDtoProfile : Profile
    {
        public MenuItemDtoProfile()
        {
            CreateMap<MenuItemRequestDto, MenuItem>().ReverseMap();
            CreateMap<MenuItemResponseDto, MenuItem>().ReverseMap();
        }
    }
}
