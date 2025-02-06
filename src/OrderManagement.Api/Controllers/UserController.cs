using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces;
using OrderManagement.Contracts.Users;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UserController(IUserService userService, IMapper mapper) : ControllerBase
    {
        [SwaggerOperation(Summary = "Gets all active users", Description = "Retrieves all active users and returns them by page")]
        [SwaggerResponse(StatusCodes.Status200OK, "Users returned", typeof(PaginatedResult<UserResponseDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Users not found")]
        [HttpGet]
        public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 10)
        {
            var result = await userService.GetAllAsync(page, pageSize);
            if (!result.IsSuccess) return NotFound(new { message = result.Error });

            var response = new PaginatedResult<UserResponseDto>(
                mapper.Map<IEnumerable<UserResponseDto>>(result.Value!.Items),
                result.Value.Page,
                result.Value.PageSize,
                result.Value.TotalCount);

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Get a specific user", Description = "Get the details of a specific user")]
        [SwaggerResponse(StatusCodes.Status200OK, "User returned", typeof(UserResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await userService.GetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(new { message = result.Error });

            return Ok(mapper.Map<UserResponseDto>(result.Value!));
        }

        [SwaggerOperation(Summary = "Get a specific user by using e-mail", Description = "Get the details of a specific user filtering by e-mail")]
        [SwaggerResponse(StatusCodes.Status200OK, "User returned", typeof(UserResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var result = await userService.GetByEmailAsync(email);
            if (!result.IsSuccess) return NotFound(new { message = result.Error });

            return Ok(mapper.Map<UserResponseDto>(result.Value!));
        }

        [SwaggerOperation(Summary = "Create a user", Description = "Create a new user")]
        [SwaggerResponse(StatusCodes.Status201Created, "User returned", typeof(UserResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "User couldn't be created")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRequestDto dto)
        {
            var user = mapper.Map<User>(dto);
            user.PasswordHash = PasswordHasher.HashPassword(dto.Password);

            var result = await userService.CreateAsync(user);
            if (result.IsFailure) return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id },
                mapper.Map<UserResponseDto>(result.Value));
        }

        [SwaggerOperation(Summary = "Update an existing user", Description = "Update an existing user")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Errors in the payload")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User couldn't be found")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserRequestDto userDto)
        {
            if (userDto.Id != id)
                return BadRequest(new { message = "Id in the request body does not match the id in the route." });

            var user = mapper.Map<User>(userDto);
            var result = await userService.UpdateAsync(user);
            if (result.IsFailure) return NotFound(new { message = result.Error });
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete an existing user", Description = "Mark as deleted an existing user")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User deleted")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User couldn't be found")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userService.DeleteAsync(id);
            if (result.IsFailure) return NotFound(new { message = result.Error });
            return NoContent();
        }
    }
}
