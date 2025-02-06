using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Interfaces;
using OrderManagement.Contracts.Orders;
using OrderManagement.Domain.Common;
using OrderManagement.Domain;
using OrderManagement.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class OrderController(IOrderService orderService, IOrderStatusService orderStatusService, IMapper mapper) : ControllerBase
    {
        [SwaggerOperation(Summary = "Gets all orders", Description = "Retrieves all orders and returns them by page. The content is poor for efficiency reasons")]
        [SwaggerResponse(StatusCodes.Status200OK, "Orders returned", typeof(PaginatedResult<OrderResponseCompactDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No orders found")]
        [HttpGet]
        public IActionResult GetOrders(int? customerId = null,
            [FromQuery] List<int>? statusIds = null,
            int page = 1,
            int pageSize = 10)
        {
            var result = orderService.GetOrdersWithFilters(customerId, statusIds, page, pageSize);
            if (!result.IsSuccess) return NotFound(new { message = result.Error });

            var response = new PaginatedResult<OrderResponseCompactDto>(
                mapper.Map<IEnumerable<OrderResponseCompactDto>>(result.Value!.Items),
                result.Value.Page,
                result.Value.PageSize,
                result.Value.TotalCount);

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Gets a specific order", Description = "Retrieves a specific order. the content is details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order returned")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The order couldn't be found")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await orderService.GetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(new { message = result.Error });

            return Ok(mapper.Map<OrderResponseDto>(result.Value!));
        }

        [SwaggerOperation(Summary = "Create an order", Description = "Create a new order")]
        [SwaggerResponse(StatusCodes.Status201Created, "Order created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error in payload validation")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequestDto dto)
        {
            var order = mapper.Map<Order>(dto);
            var result = await orderService.CreateAsync(order);
            if (result.IsFailure) return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id },
                mapper.Map<OrderResponseDto>(result.Value));
        }

        [SwaggerOperation(Summary = "Update an order", Description = "Update an existing order that hasn't started preparation")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Order updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error in payload validation")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order couldn't be found")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderRequestDto orderDto)
        {
            if (orderDto.Id != id)
                return BadRequest(new { message = "Id in the request body does not match the id in the route." });

            var order = mapper.Map<Order>(orderDto);
            var result = await orderService.UpdateAsync(order);
            if (result.IsFailure) return NotFound(new { message = result.Error });

            return NoContent();
        }

        [SwaggerOperation(Summary = "Move the order to the next status", Description = "The employee moves the order to the next status")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Order moved to the next status")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "No more statuses available for this order")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> MoveToNextStatus(int id)
        {
            var result = await orderService.UpdateOrderStatusAsync(id);
            if (!result.IsSuccess) return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [SwaggerOperation(Summary = "Assign an order to a delivery person", Description = "Assign an order marked for delivery to a delivery person")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Order assigned successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Order cannot be assigned to a delivery person")]

        [HttpPut("{id}/assign/{deliveryUserId}")]
        public async Task<IActionResult> AssignOrderToDelivery(int id, int deliveryUserId)
        {
            var result = await orderService.AssignOrderToDeliveryAsync(id, deliveryUserId);
            if (!result.IsSuccess) return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [SwaggerOperation(Summary = "Cancel an order", Description = "Cancel an order that hasn't started preparation")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Order deleted successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Order couldn't be deleted")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await orderService.DeleteAsync(id);
            if (result.IsFailure) return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [SwaggerOperation(Summary = "Get the selected menu items for a specific order", Description = "Get the selected menu items for a specific order")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of items returned", typeof(OrderItemResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Order couldn't be deleted")]
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetOrderItems(int id)
        {
            var result = await orderService.GetOrderItemsAsync(id);
            if (!result.IsSuccess) return NotFound(new { message = result.Error });

            return Ok(mapper.Map<IEnumerable<OrderItemResponseDto>>(result.Value!));
        }

        [SwaggerOperation(Summary = "Get the list of statuses for a specific order", Description = "Get analytically the list of statuses for a specific order")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of statuses returned", typeof(OrderStatusResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order couldn't be found")]
        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetOrderStatuses(int id)
        {
            var result = await orderStatusService.GetByOrderIdAsync(id);
            if (!result.IsSuccess) return NotFound(new { message = result.Error });

            return Ok(mapper.Map<IEnumerable<OrderStatusResponseDto>>(result.Value!));
        }

        [SwaggerOperation(Summary = "Get the list of available statuses for a specific type of order", Description = "Get the list of available statuses for a specific type of order")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of statuses returned", typeof(IEnumerable<OrderStatusesResponseDto>))]
        [HttpGet("statuses")]
        public IActionResult GetAvailableStatuses([FromQuery] OrderTypeEnum orderTypeId)
        {
            return Ok(orderStatusService.GetAvailableStatuses(orderTypeId)
                .Select(x => new OrderStatusesResponseDto(x, Enum.GetName(typeof(OrderStatusEnum), x))));
        }
    }
}
