using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Common;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Enums;
using OrderManagement.Infrastructure.DataAccess.DbContexts;

namespace OrderManagement.Infrastructure.Reporting
{
    //TODO: Review the queries performance on large datasets

    public class ReportingRepository(OrderManagementDbContext context) : IReportingRepository
    {
        // Average Fulfillment Time (Created -> Ready for Pickup/Delivery)
        public async Task<Result<double>> GetAverageFulfillmentTimeAsync(DateTime startDate, DateTime endDate)
    {
        var averageTime = await context.Orders
            .Where(o => o.DateTimeCreated >= startDate && o.DateTimeCreated <= endDate)
            .Where(o => context.OrderStatuses.Any(os =>
                os.OrderId == o.Id &&
                (os.OrderStatusId == (int)OrderStatusEnum.ReadyForPickup || os.OrderStatusId == (int)OrderStatusEnum.ReadyForDelivery)))
            .Select(o =>
                context.OrderStatuses
                    .Where(os => os.OrderId == o.Id && (os.OrderStatusId == (int)OrderStatusEnum.ReadyForPickup || os.OrderStatusId == (int)OrderStatusEnum.ReadyForDelivery))
                    .OrderByDescending(os => os.DateTimeCreated)
                    .Select(os => os.DateTimeCreated)
                    .FirstOrDefault() - o.DateTimeCreated)
            .AverageAsync(ts => ts.TotalMinutes);

        return Result<double>.Success(averageTime);
    }

    // Average Delivery Time (Out for Delivery -> Delivered)
    public async Task<Result<double>> GetAverageDeliveryTimeAsync(DateTime startDate, DateTime endDate)
    {
        var averageTime = await context.Orders
            .Where(o => o.DateTimeCreated >= startDate && o.DateTimeCreated <= endDate)
            .Where(o => context.OrderStatuses.Any(os =>
                os.OrderId == o.Id && os.OrderStatusId == (int)OrderStatusEnum.OutForDelivery))
            .Select(o =>
                context.OrderStatuses
                    .Where(os => os.OrderId == o.Id && os.OrderStatusId == (int)OrderStatusEnum.Delivered)
                    .OrderByDescending(os => os.DateTimeCreated)
                    .Select(os => os.DateTimeCreated)
                    .FirstOrDefault() -
                context.OrderStatuses
                    .Where(os => os.OrderId == o.Id && os.OrderStatusId == (int)OrderStatusEnum.OutForDelivery)
                    .OrderByDescending(os => os.DateTimeCreated)
                    .Select(os => os.DateTimeCreated)
                    .FirstOrDefault())
            .AverageAsync(ts => ts.TotalMinutes);

        return Result<double>.Success(averageTime);
    }

    // Percentage of "Unable to Deliver" Orders vs. Delivered Orders
    public async Task<Result<double>> GetUnableToDeliverPercentageAsync(DateTime startDate, DateTime endDate)
    {
        var deliveredCount = await context.OrderStatuses
            .Where(os => os.DateTimeCreated >= startDate && os.DateTimeCreated <= endDate)
            .CountAsync(os => os.OrderStatusId == (int)OrderStatusEnum.Delivered);

        var unableToDeliverCount = await context.OrderStatuses
            .Where(os => os.DateTimeCreated >= startDate && os.DateTimeCreated <= endDate)
            .CountAsync(os => os.OrderStatusId == (int)OrderStatusEnum.UnableToDeliver);

        if (deliveredCount == 0)
            return Result<double>.Success(0);

        var percentage = (unableToDeliverCount / (double)deliveredCount) * 100;
        return Result<double>.Success(percentage);
    }

    // Pickup vs. Delivery Ratio
    public async Task<Result<double>> GetPickupVsDeliveryRatioAsync(DateTime startDate, DateTime endDate)
    {
        var pickupCount = await context.Orders
            .Where(o => o.DateTimeCreated >= startDate && o.DateTimeCreated <= endDate)
            .CountAsync(o => o.OrderTypeId == (int)OrderTypeEnum.Pickup);

        var deliveryCount = await context.Orders
            .Where(o => o.DateTimeCreated >= startDate && o.DateTimeCreated <= endDate)
            .CountAsync(o => o.OrderTypeId == (int)OrderTypeEnum.Delivery);

        if (deliveryCount == 0)
            return Result<double>.Success(100);

        var ratio = (pickupCount / (double)deliveryCount) * 100;
        return Result<double>.Success(ratio);
    }
}
}
