using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Interfaces;
using OrderManagement.Contracts.Customers;
using OrderManagement.Domain;
using OrderManagement.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CustomerController(ICustomerService customerService, IMapper mapper) : ControllerBase
    {
        [SwaggerOperation(Summary = "Gets all active customer", Description = "Retrieves all active customers and returns them by page")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer returned", typeof(PaginatedResult<CustomerResponseDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found")]
        [HttpGet]
        public async Task<IActionResult> GetCustomers(int page = 1, int size = 10)
        {
            var result = await customerService.GetAllAsync(page, size);
            if (result.IsFailure) return BadRequest(new { message = result.Error });

            var customersDto = mapper.Map<IEnumerable<CustomerResponseDto>>(result.Value.Items);

            var paginatedResult = new PaginatedResult<CustomerResponseDto>(customersDto,
                result.Value.Page, result.Value.PageSize, result.Value.TotalCount);

            return Ok(paginatedResult);
        }


        [SwaggerOperation(Summary = "Gets a customer by ID", Description = "Retrieves a single user based on the provided ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer returned", typeof(CustomerResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var result = await customerService.GetByIdAsync(id);
            if (result.IsFailure) return NotFound(new { message = result.Error });
            return Ok(mapper.Map<CustomerResponseDto>(result.Value));
        }

        [SwaggerOperation(Summary = "Create a new customer", Description = "Create a new customer and connecting him with an existing user")]
        [SwaggerResponse(StatusCodes.Status201Created, "Customer created", typeof(CustomerResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload received")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequestDto customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);
            var result = await customerService.CreateAsync(customer);
            if (result.IsFailure) return BadRequest(new { message = result.Error });
            return CreatedAtAction(nameof(CreateCustomer), new { id = result.Value.Id }, mapper.Map<CustomerResponseDto>(result.Value));
        }

        [SwaggerOperation(Summary = "Modify customer data", Description = "Modify customer data")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Customer updated successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerRequestDto customerDto)
        {
            if (customerDto.Id != id)
                return BadRequest(new { message = "Id in the request body does not match the id in the route." });

            var customer = mapper.Map<Customer>(customerDto);
            var result = await customerService.UpdateAsync(customer);
            if (result.IsFailure) return NotFound(new { message = result.Error });
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete the customer", Description = "Mark the customer as deleted")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Customer updated successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await customerService.DeleteAsync(id);
            if (result.IsFailure) return NotFound(new { message = result.Error });
            return NoContent();
        }
    }
}