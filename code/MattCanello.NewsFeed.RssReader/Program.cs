using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using MattCanello.NewsFeed.RssReader.Domain.Application;
using MattCanello.NewsFeed.RssReader.Domain.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Handlers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Clients;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Enrichers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Handlers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Repositories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services;
using MattCanello.NewsFeed.RssReader.Domain.Profiles;
using MattCanello.NewsFeed.RssReader.Domain.Services;
using MattCanello.NewsFeed.RssReader.Infrastructure.Clients;
using MattCanello.NewsFeed.RssReader.Infrastructure.EventPublishers;
using MattCanello.NewsFeed.RssReader.Infrastructure.Factories;
using MattCanello.NewsFeed.RssReader.Infrastructure.Filters;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.RssReader
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

            builder.Services.AddMapperProfiles();

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

            app.Run();
        }

        private static void AddAppServices(this IServiceCollection services)
        {
            services
                .AddScoped<IFeedConsumedPublisher, DaprFeedConsumedPublisher>()
                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddScoped<IChannelService, ChannelService>();

            services
                .AddScoped<INewEntryFoundPublisher, DaprNewEntryFoundPublisher>()
                .AddSingleton<IEntryFactory, EntryFactory>()
                .AddScoped<IEntryService, EntryService>();

            services
                .AddSingleton<INonStandardEnricherEvaluator, NonStandardEnricherEvaluator>();

            services
                .AddSingleton<IFeedRepository, DaprFeedRepository>();

            services
                .AddSingleton<ICreateFeedHandler, CreateFeedHandler>();

            services
                .AddHttpClient()
                .AddScoped<IRssClient, RssClient>()
                .AddScoped<IRssApp, RssApp>();
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

        private static void AddMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper((config) =>
            {
                config.AddProfile<FeedProfile>();
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
