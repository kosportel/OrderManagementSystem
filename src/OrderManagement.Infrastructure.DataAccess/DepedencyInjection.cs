using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Infrastructure.DataAccess.DbContexts;

namespace OrderManagement.Infrastructure.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddPersistence(configuration);
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Get connection string from the selected appsettings file
        var connectionString = configuration.GetConnectionString("OrderManagementConnection");

        services.AddDbContext<OrderManagementDbContext>(options => options.UseSqlite(connectionString));

        return services;
    }
}