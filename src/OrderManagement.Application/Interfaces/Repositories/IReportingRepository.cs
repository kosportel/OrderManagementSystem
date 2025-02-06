﻿using OrderManagement.Application.Common;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IReportingRepository
    {
        Task<Result<double>> GetAverageFulfillmentTimeAsync(DateTime startDate, DateTime endDate);
        Task<Result<double>> GetAverageDeliveryTimeAsync(DateTime startDate, DateTime endDate);
        Task<Result<double>> GetUnableToDeliverPercentageAsync(DateTime startDate, DateTime endDate);
        Task<Result<double>> GetPickupVsDeliveryRatioAsync(DateTime startDate, DateTime endDate);
    }
}
