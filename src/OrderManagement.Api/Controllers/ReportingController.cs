using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagement.Api.Controllers
{
    // TODO: Strict the 

    [ApiController]
    [Route("api/reporting")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [SwaggerOperation(Summary = "Average Fulfillment Time (Created -> Ready for Pickup/Delivery)")]
        [HttpGet("average-fulfillment-time")]
        public async Task<IActionResult> GetAverageFulfillmentTime([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportingService.GetAverageFulfillmentTimeAsync(startDate, endDate);
            return result.IsSuccess ? Ok(new { averageFulfillmentTimeMinutes = result.Value }) : BadRequest(result.Error);
        }

        [SwaggerOperation(Summary = "Average Delivery Time (Out for Delivery -> Delivered)")]
        [HttpGet("average-delivery-time")]
        public async Task<IActionResult> GetAverageDeliveryTime([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportingService.GetAverageDeliveryTimeAsync(startDate, endDate);
            return result.IsSuccess ? Ok(new { averageDeliveryTimeMinutes = result.Value }) : BadRequest(result.Error);
        }

        [SwaggerOperation(Summary = "Percentage of \"Unable to Deliver\" Orders vs. Delivered Orders")]
        [HttpGet("unable-to-deliver-percentage")]
        public async Task<IActionResult> GetUnableToDeliverPercentage([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportingService.GetUnableToDeliverPercentageAsync(startDate, endDate);
            return result.IsSuccess ? Ok(new { unableToDeliverPercentage = result.Value }) : BadRequest(result.Error);
        }

        [SwaggerOperation(Summary = "Pickup vs. Delivery Ratio")]
        [HttpGet("order-type-ratio")]
        public async Task<IActionResult> GetPickupVsDeliveryRatio([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportingService.GetPickupVsDeliveryRatioAsync(startDate, endDate);
            return result.IsSuccess ? Ok(new { pickupVsDeliveryPercentage = result.Value }) : BadRequest(result.Error);
        }
    }
}
