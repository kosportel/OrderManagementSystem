using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.Services
{
    public class ReportingService(IReportingRepository reportingRepository) : IReportingService
    {
        public async Task<Result<double>> GetAverageFulfillmentTimeAsync(DateTime startDate, DateTime endDate)
        {
            return await reportingRepository.GetAverageFulfillmentTimeAsync(startDate, endDate);
        }

        public async Task<Result<double>> GetAverageDeliveryTimeAsync(DateTime startDate, DateTime endDate)
        {
            return await reportingRepository.GetAverageDeliveryTimeAsync(startDate, endDate);
        }

        public async Task<Result<double>> GetUnableToDeliverPercentageAsync(DateTime startDate, DateTime endDate)
        {
            return await reportingRepository.GetUnableToDeliverPercentageAsync(startDate, endDate);
        }

        public async Task<Result<double>> GetPickupVsDeliveryRatioAsync(DateTime startDate, DateTime endDate)
        {
            return await reportingRepository.GetPickupVsDeliveryRatioAsync(startDate, endDate);
        }
    }
}
