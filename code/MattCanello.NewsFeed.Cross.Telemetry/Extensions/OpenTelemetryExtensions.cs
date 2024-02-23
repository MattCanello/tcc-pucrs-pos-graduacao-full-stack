using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MattCanello.NewsFeed.Cross.Telemetry.Extensions
{
    public static class OpenTelemetryExtensions
    {
        public static OpenTelemetryBuilder? AddDefaultTelemetry(this WebApplicationBuilder builder)
        {
            var otel = builder.Services.AddOpenTelemetry();
            var isDev = builder.Environment.IsDevelopment();

            var appInsightsConnStr = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
            var hasAppInsights = !string.IsNullOrEmpty(appInsightsConnStr);

            if (hasAppInsights)
                otel.UseAzureMonitor();

            otel.ConfigureResource(resource => resource
                .AddService(serviceName: builder.Environment.ApplicationName));

            otel.WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel");

                if (isDev)
                    metrics.AddConsoleExporter();
            });

            otel.WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();

                if (isDev)
                    tracing.AddConsoleExporter();

                if (hasAppInsights)
                    tracing.AddAzureMonitorTraceExporter();
            });

            return otel;
        }
    }
}
