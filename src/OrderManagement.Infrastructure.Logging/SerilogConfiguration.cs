using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace OrderManagement.Infrastructure.Logging
{
    public static class SerilogConfiguration
    {
        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            hostBuilder.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(configuration) // Load from appsettings.json
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext() // Required for Correlation ID
                    .Enrich.WithMachineName() // Adds machine name to logs
                    .Enrich.WithThreadId() // Adds Thread ID for debugging
                    .Enrich.WithProcessId() // Adds Process ID
                    .Enrich.WithProperty("Application", "OrderManagement"); // Custom property
            });

            return hostBuilder;
        }
    }
}
