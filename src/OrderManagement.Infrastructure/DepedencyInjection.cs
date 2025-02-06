using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Infrastructure.Common;
using OrderManagement.Infrastructure.Customers.Persistence;
using OrderManagement.Infrastructure.Menu.Persistence;
using OrderManagement.Infrastructure.Orders.Persistence;
using OrderManagement.Infrastructure.Users.Persistence;

namespace OrderManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddRepositories();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
        services.AddScoped<IOrderAssignmentRepository, OrderAssignmentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}