using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Filters;
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
using MattCanello.NewsFeed.RssReader.Infrastructure.Decorators;
using MattCanello.NewsFeed.RssReader.Infrastructure.Evaluators;
using MattCanello.NewsFeed.RssReader.Infrastructure.EventPublishers;
using MattCanello.NewsFeed.RssReader.Infrastructure.Factories;
using MattCanello.NewsFeed.RssReader.Infrastructure.Filters;
using MattCanello.NewsFeed.RssReader.Infrastructure.Formatters.Rss091;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Evaluators;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Infrastructure.Repositories;
using MattCanello.NewsFeed.RssReader.Infrastructure.Strategies;
using MattCanello.NewsFeed.RssReader.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;
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
            builder.Services.AddAppServices();
            builder.Services.ConfigureHealthChecks();

            builder.AddDefaultTelemetry(
                metrics => metrics.AddMeter(Metrics.PublishedEntriesCount.Name),
                tracing => tracing.AddSource(ActivitySources.RssApp.Name));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/app/health");

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
               .AddScoped<Rss091Formatter>()
               .AddSingleton<DefaultSyndicationFeedLoader>()
               .AddScoped<Rss091SyndicationFeedLoader>()
               .AddScoped<ISyndicationFeedEvaluator, SyndicationFeedEvaluator>();

            services
                .AddHttpClient()
                .AddScoped<IRssClient, RssClient>()
                .AddScoped<IRssApp, RssApp>()
                .Decorate<IRssApp, RssAppLogDecorator>()
                .Decorate<IRssApp, RssAppMetricsDecorator>();

            services
                .AddSingleton<IEventFactory, EventFactory>();
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

        private static void AddMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper((config) =>
            {
                config.AddProfile<FeedProfile>();
            });
        }
    }
}
