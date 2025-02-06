using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Contracts.Orders;
using OrderManagement.Application.Interfaces;
using OrderManagement.Contracts.Users;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/delivery")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IMapper _mapper;

        public DeliveryController(IDeliveryService deliveryService, IMapper mapper)
        {
            _deliveryService = deliveryService;
            _mapper = mapper;
        }

        [SwaggerOperation(Summary = "Get all the assigned orders for delivery", Description = "Retrieves all assigned orders ready for delivery")]
        [SwaggerResponse(StatusCodes.Status200OK, "Results returned")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error in finding results")]
        [HttpGet("assignments")]
        public async Task<IActionResult> GetAllAssignedDeliveries()
        {
            var result = await _deliveryService.GetAllAssignedDeliveriesAsync();
            if (!result.IsSuccess) return BadRequest(new { message = result.Error });

            return Ok(_mapper.Map<IEnumerable<OrderAssignmentResponseDto>>(result.Value));
        }


        [SwaggerOperation(Summary = "Get all the assigned orders for a specific Delivery person", Description = "Retrieves all assigned orders for a specific delivery person")]
        [SwaggerResponse(StatusCodes.Status200OK, "Results returned")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error in finding results")]
        [HttpGet("assignments/user/{id}")]
        public async Task<IActionResult> GetAssignedDeliveries(int id)
        {
            var result = await _deliveryService.GetAssignedDeliveriesAsync(id);
            if (!result.IsSuccess) return BadRequest(new { message = result.Error });

            return Ok(_mapper.Map<IEnumerable<OrderResponseDto>>(result.Value));
        }

        [SwaggerOperation(Summary = "Get the Delivery persons that don't have any assignments", Description = "Get the idle persons, to assign them new orders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Results returned", typeof(IEnumerable<UserResponseDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error in finding results")]
        [HttpGet("idle")]
        public async Task<IActionResult> GetIdleDeliveryPeople()
        {
            var result = await _deliveryService.GetIdleDeliveryPeopleAsync();
            if (!result.IsSuccess) return BadRequest(new { message = result.Error });

            return Ok(_mapper.Map<IEnumerable<UserResponseDto>>(result.Value));
        }

        [SwaggerOperation(Summary = "Mark the order as Out for Delivery", Description = "The delivery person, takes the order Out for Delivery")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Update succeeds")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error in the process")]
        [HttpPut("orders/{id}/startDelivery")]
        public async Task<IActionResult> StartDelivery(int id, int userId)
        {
            var result = await _deliveryService.StartDeliveryAsync(id, userId);
            if (!result.IsSuccess) return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [SwaggerOperation(Summary = "Mark the order as delivered or not", Description = "The delivery person, sets the final status for the order (Delivered or Not)")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Results returned")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error in the process")]
        [HttpPut("orders/{id}/close")]
        public async Task<IActionResult> UpdateDeliveryStatus(int id, [FromBody] UpdateDeliveryStatusDto dto)
        {
            var result = await _deliveryService.UpdateDeliveryStatusAsync(id, dto.UserId, dto.IsDelivered);
            if (!result.IsSuccess) return BadRequest(new { message = result.Error });

            return NoContent();
        }
    }
}
