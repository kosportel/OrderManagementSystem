using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;

namespace OrderManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddServices();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IMenuItemService, MenuItemService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderStatusService, OrderStatusService>();
        services.AddScoped<IDeliveryService, DeliveryService>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddSingleton<IOrderStrategyFactory, OrderStrategyFactory>();

        return services;
    }
}