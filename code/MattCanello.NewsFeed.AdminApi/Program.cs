using MattCanello.NewsFeed.AdminApi.Domain.Application;
using MattCanello.NewsFeed.AdminApi.Domain.Decorators;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Services;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Decorators;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Repositories;
using MattCanello.NewsFeed.AdminApi.Infrastructure.EventPublishers;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Factories;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Filters;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry;
using MattCanello.NewsFeed.Cross.Abstractions;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.ElasticSearch.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.AdminApi
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

            builder.Services.AddResponseCaching();
            builder.Services.AddResponseCompression();

            builder.AddDefaultTelemetry(
                configureTracing: (builder) => builder.AddSource(ActivitySources.FeedApp.Name, ActivitySources.ChannelApp.Name));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapGet("/", () => Results.LocalRedirect("/swagger"));
            }

            app.UseAuthorization();

            app.UseResponseCompression();

            app.UseResponseCaching();

            app.MapControllers();

            app.MapHealthChecks("/app/health");

            app.Run();
        }

        private static void AddAppServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

            services
                .AddScoped<IFeedApp, FeedApp>()
                .AddScoped<IChannelApp, ChannelApp>()
                .AddScoped<IChannelService, ChannelService>();

            services
                .AddElasticSearch()
                .AddMemoryCache();

            services
                .AddScoped<IElasticSearchManagementRepository, ElasticSearchRepository>()
                .AddScoped<IElasticSearchRepository, ElasticSearchRepository>()
                .AddScoped<IFeedRepository, ElasticFeedRepository>()
                .AddScoped<IChannelRepository, ElasticChannelRepository>();

            services
                .AddScoped<IEventFactory, EventFactory>()
                .AddScoped<IEventPublisher, DaprEventPublisher>();

            services
                .Decorate<IElasticSearchManagementRepository, CachedElasticSearchManagementRepository>();

            services
                .Decorate<IFeedApp, FeedAppPublishEventDecorator>()
                .Decorate<IFeedApp, FeedAppLogDecorator>()
                .Decorate<IFeedApp, FeedAppMetricsDecorator>();

            services
                .Decorate<IChannelApp, ChannelAppLogDecorator>()
                .Decorate<IChannelApp, ChannelAppMetricsDecorator>();

            services
                .AddAutoMapper(config =>
                {
                    config.AddProfile<FeedProfile>();
                    config.AddProfile<ChannelProfile>();
                });
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
