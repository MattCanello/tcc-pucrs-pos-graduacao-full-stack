using MattCanello.NewsFeed.CronApi.Domain.Applications;
using MattCanello.NewsFeed.CronApi.Domain.Handlers;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Domain.Services;
using MattCanello.NewsFeed.CronApi.Infrastructure.Decorators;
using MattCanello.NewsFeed.CronApi.Infrastructure.Enqueuers;
using MattCanello.NewsFeed.CronApi.Infrastructure.Factories;
using MattCanello.NewsFeed.CronApi.Infrastructure.Filters;
using MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.CronApi.Infrastructure.Repositories;
using MattCanello.NewsFeed.CronApi.Infrastructure.Telemetry;
using MattCanello.NewsFeed.Cross.Abstractions;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.CronApi
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDefaultControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDapr();
            builder.Services.AddAppServices();
            builder.Services.ConfigureHealthChecks();

            builder.AddDefaultTelemetry(
                metrics => metrics.AddMeter(Metrics.PublishedSlotsCount.Name),
                tracing => tracing.AddSource(ActivitySources.CronPublishApp.Name));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapGet("/", () => Results.LocalRedirect("/swagger"));
            }

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/app/health");

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
                .AddScoped<ICronPublishApp, CronPublishApp>()
                .Decorate<ICronPublishApp, CronPublishAppLogDecorator>()
                .Decorate<ICronPublishApp, CronPublishAppMetricsDecorator>();

            services
                .AddSingleton<IEventFactory, EventFactory>()
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
    }
}
