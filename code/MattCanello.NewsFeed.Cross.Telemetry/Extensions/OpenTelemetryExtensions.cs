using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Monitor.OpenTelemetry.Exporter;
using MattCanello.NewsFeed.Cross.Telemetry.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MattCanello.NewsFeed.Cross.Telemetry.Extensions
{
    public static class OpenTelemetryExtensions
    {
        public static OpenTelemetryBuilder? AddDefaultTelemetry(this WebApplicationBuilder builder, 
            Action<MeterProviderBuilder>? configureMetrics = null, 
            Action<TracerProviderBuilder>? configureTracing = null)
        {
            builder.Services.AddLogging((logBuilder) =>
            {
                logBuilder.AddSimpleConsole(opt =>
                {
                    opt.TimestampFormat = "[yyyy-MM-dd HH:mm:ss UTC] ";
                    opt.UseUtcTimestamp = true;
                });
            });

            var otel = builder.Services.AddOpenTelemetry();
            var isDev = builder.Environment.IsDevelopment();

            var hasAppInsights = EnvironmentVariables
                .HasApplicationInsightsConnectionString(out string? appInsightsConnStr);

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

                configureMetrics
                    ?.Invoke(metrics);

                if (isDev)
                    metrics.AddConsoleExporter();

                if (hasAppInsights)
                    metrics.AddAzureMonitorMetricExporter();
            });

            otel.WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();

                configureTracing
                    ?.Invoke(tracing);

                if (isDev)
                    tracing.AddConsoleExporter();

                if (hasAppInsights)
                    tracing.AddAzureMonitorTraceExporter();
            });

            return otel;
        }
    }
}
