using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Infrastructure.DataAccess.DbContexts;

namespace OrderManagement.Infrastructure.DataAccess.Management
{
    public static class MigrationManager
    {
        public static IServiceProvider Migrate(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderManagementDbContext>();
            dbContext.Database.Migrate(); // Apply migrations automatically if needed

            return services;
        }
    }
}
