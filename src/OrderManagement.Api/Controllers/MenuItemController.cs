using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Interfaces;
using OrderManagement.Contracts.Customers;
using OrderManagement.Contracts.MenuItem;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/menu-items")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IMapper _mapper;

        public MenuItemController(IMenuItemService menuItemService, IMapper mapper)
        {
            _menuItemService = menuItemService;
            _mapper = mapper;
        }

        [SwaggerOperation(Summary = "Gets all active menu items", Description = "Retrieves all active menu items and returns them by page")]
        [SwaggerResponse(StatusCodes.Status200OK, "Menu items returned", typeof(PaginatedResult<MenuItemResponseDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error in the process")]
        [HttpGet]
        public async Task<IActionResult> GetMenuItems(int page = 1, int pageSize = 10)
        {
            var result = await _menuItemService.GetAllAsync(page, pageSize);
            if (!result.IsSuccess) return BadRequest(new { message = result.Error });

            var response = new PaginatedResult<MenuItemResponseDto>(
                _mapper.Map<IEnumerable<MenuItemResponseDto>>(result.Value!.Items),
                result.Value.Page,
                result.Value.PageSize,
                result.Value.TotalCount);

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Gets a specific menu item", Description = "Search for a specific active menu items")]
        [SwaggerResponse(StatusCodes.Status200OK, "Menu item returned", typeof(MenuItemResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Item couldn't be found")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _menuItemService.GetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(new { message = result.Error });

            return Ok(_mapper.Map<MenuItemResponseDto>(result.Value!));
        }


        [SwaggerOperation(Summary = "Create a new menu item", Description = "Create a new menu item")]
        [SwaggerResponse(StatusCodes.Status201Created, "Menu item created", typeof(MenuItemResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with the validation of the payload")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuItemRequestDto dto)
        {
            var menuItem = _mapper.Map<MenuItem>(dto);
            var result = await _menuItemService.CreateAsync(menuItem);
            if (result.IsFailure) return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id },
                _mapper.Map<MenuItemResponseDto>(result.Value));
        }

        [SwaggerOperation(Summary = "Update the menu item", Description = "Update the menu item")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Menu item updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with the validation of the payload")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Item couldn't be found")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuItemRequestDto menuItemDto)
        {
            if (menuItemDto.Id != id)
                return BadRequest(new { message = "Id in the request body does not match the id in the route." });

            var menuItem = _mapper.Map<MenuItem>(menuItemDto);
            var result = await _menuItemService.UpdateAsync(menuItem);
            if (result.IsFailure) return NotFound(new { message = result.Error });
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete the menu item", Description = "Mark the menu item as deleted")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Menu item deleted")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Item couldn't be found")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _menuItemService.DeleteAsync(id);
            if (result.IsFailure) return NotFound(new { message = result.Error });
            return NoContent();
        }
    }
}
