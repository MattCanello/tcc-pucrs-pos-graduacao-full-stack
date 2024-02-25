using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Monitor.OpenTelemetry.Exporter;
using MattCanello.NewsFeed.Cross.Telemetry.Utils;
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

            var hasAppInsights = EnvironmentVariables
                .HasApplicationInsightsConnectionString(out string? appInsightsConnStr);

            if (hasAppInsights)
                otel.UseAzureMonitor();

            otel.ConfigureResource(resource => resource
                .AddService(serviceName: builder.Environment.ApplicationName));

            var tracingOtlpEndpoint = builder.Configuration
                .GetOpenTelemetryEndpointUrl();

            otel.WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel");

                if (isDev)
                    metrics.AddConsoleExporter();

                if (hasAppInsights)
                    metrics.AddAzureMonitorMetricExporter();

                if (tracingOtlpEndpoint != null)
                    metrics.AddOtlpExporter(options => options.Endpoint = new Uri(tracingOtlpEndpoint));

                metrics.AddPrometheusExporter();
            });

            otel.WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();

                if (isDev)
                    tracing.AddConsoleExporter();

                if (hasAppInsights)
                    tracing.AddAzureMonitorTraceExporter();

                if (tracingOtlpEndpoint != null)
                    tracing.AddOtlpExporter(options => options.Endpoint = new Uri(tracingOtlpEndpoint));
            });

            return otel;
        }
    }
}
