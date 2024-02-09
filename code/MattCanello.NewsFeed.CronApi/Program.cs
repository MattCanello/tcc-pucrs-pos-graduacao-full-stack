using CloudNative.CloudEvents.SystemTextJson;
using CloudNative.CloudEvents;
using MattCanello.NewsFeed.CronApi.Domain.Applications;
using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Handlers;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Domain.Services;
using MattCanello.NewsFeed.CronApi.Infrastructure.Enqueuers;
using MattCanello.NewsFeed.CronApi.Infrastructure.Filters;
using MattCanello.NewsFeed.CronApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.CronApi.Infrastructure.Factories;

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
            builder.Services.AddCloudEvents();
            builder.Services.AddAppServices();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.MapCronEndpoint();

            app.Run();
        }

        private static void AddAppServices(this IServiceCollection services)
        {
            services
                .AddScoped<IRegisterFeedHandler, RegisterFeedHandler>()
                .AddScoped<ISlotCounterService, SequentialSlotCounterService>();

            services
                .AddScoped<IFeedRepository, DaprFeedRepository>()
                .AddScoped<ISlotRepository, DaprSlotRepository>();

            services
                .AddScoped<ICronPublishApp, CronPublishApp>()
                .AddScoped<ICronFeedEnqueuer, DaprCronFeedEnqueuer>();
        }

        private static void AddDefaultControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.RemoveType<StringOutputFormatter>();

                options.Filters.Add<HttpExceptionFilter>();
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

        private static void MapCronEndpoint(this WebApplication app)
        {
            app.MapPost("/publish", async ([FromQuery] byte slot, [FromServices] ICronPublishApp cronPublishApp) =>
            {
                SlotOutOfRangeException.ThrowIfOutOfRange(slot);

                await cronPublishApp!.PublishSlotAsync(slot);

                return Results.NoContent();
            });
        }

        private static void AddCloudEvents(this IServiceCollection services)
        {
            services
                .AddSingleton<ICloudEventFactory, CloudEventFactory>()
                .AddSingleton<CloudEventFormatter, JsonEventFormatter>((s) =>
                {
                    return new JsonEventFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                    }, new JsonDocumentOptions());
                });
        }
    }
}
