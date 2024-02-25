using MattCanello.NewsFeed.Cross.Telemetry.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace MattCanello.NewsFeed.Cross.Telemetry.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder? ConfigureHealthChecks(this IServiceCollection services)
        {
            var healthCheckBuilder = services
                .AddHealthChecks();

            if (EnvironmentVariables.HasApplicationInsightsConnectionString(out _))
                healthCheckBuilder.AddApplicationInsightsPublisher();

            return healthCheckBuilder;
        }
    }
}
