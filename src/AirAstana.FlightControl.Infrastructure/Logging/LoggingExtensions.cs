using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AirAstana.FlightControl.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((ctx, lc) =>
        {
            lc.ReadFrom.Configuration(configuration);
        });

        return hostBuilder;
    }
}