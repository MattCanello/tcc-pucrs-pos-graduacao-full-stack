using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using MattCanello.NewsFeed.RssReader.Factories;
using MattCanello.NewsFeed.RssReader.Filters;
using MattCanello.NewsFeed.RssReader.Infrastructure;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Profiles;
using MattCanello.NewsFeed.RssReader.Services;
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
                .AddScoped<IChannelPublisher, DaprChannelPublisher>()
                .AddSingleton<IChannelReader, ChannelReader>()
                .AddScoped<IChannelService, ChannelService>();

            services
                .AddScoped<IEntryPublisher, DaprEntryPublisher>()
                .AddSingleton<IEntryReader, EntryReader>()
                .AddScoped<IEntryService, EntryService>();

            services
                .AddSingleton<INonStandardEnricherEvaluator, NonStandardEnricherEvaluator>();

            services
                .AddSingleton<IFeedRepository, DaprFeedRepository>();

            services
                .AddHttpClient()
                .AddScoped<IRssClient, RssClient>()
                .AddScoped<IRssService, RssService>();
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
