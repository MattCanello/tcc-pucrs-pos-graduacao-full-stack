using MattCanello.NewsFeed.CronApi.Domain.Applications;
using MattCanello.NewsFeed.CronApi.Domain.Handlers;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Domain.Services;
using MattCanello.NewsFeed.CronApi.Infrastructure.Enqueuers;
using MattCanello.NewsFeed.CronApi.Infrastructure.Filters;
using MattCanello.NewsFeed.CronApi.Infrastructure.Repositories;
using MattCanello.NewsFeed.Cross.Abstractions;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;
using MattCanello.NewsFeed.Cross.CloudEvents.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.CronApi
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCloudEvents();
            builder.Services.AddDefaultControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDapr();
            builder.Services.AddAppServices();
            builder.Services.ConfigureHealthChecks();

            builder.AddDefaultTelemetry();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/app/health");

            app.UseOpenTelemetryPrometheusScrapingEndpoint();

            app.Run();
        }

        private static void AddAppServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

            services
                .AddScoped<IRegisterFeedHandler, RegisterFeedHandler>()
                .AddScoped<ISlotCounterService, SequentialSlotCounterService>();

            services
                .AddScoped<IFeedRepository, DaprFeedRepository>()
                .AddScoped<ISlotRepository, DaprSlotRepository>();

            services
                .AddScoped<CronPublishApp>()
                .AddScoped<ICronPublishApp, CronPublishAppLog>()
                .AddScoped<ICronFeedEnqueuer, DaprCronFeedEnqueuer>();
        }

        private static void AddDefaultControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.RemoveType<StringOutputFormatter>();

                options.Filters.Add<HttpExceptionFilter>();
                options.Filters.Add<ActionLoggingFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
        }

        private static void AddDapr(this IServiceCollection services)
        {
            services.AddDaprClient((builder) =>
            {
                builder.UseJsonSerializationOptions(new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                });
            });
        }
    }
}
