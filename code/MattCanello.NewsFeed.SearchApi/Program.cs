using MattCanello.NewsFeed.Cross.Abstractions;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Filters;
using MattCanello.NewsFeed.SearchApi.Domain.Application;
using MattCanello.NewsFeed.SearchApi.Domain.Decorators;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Policies;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Decorators.IndexApp;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Decorators.SearchApp;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Extensions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.EventPublishers;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Factories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Filters;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.SearchApi
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
                metrics => metrics
                    .AddMeter(Metrics.IndexedDocuments.Name)
                    .AddMeter(Metrics.SearchCount.Name)
                    .AddMeter(Metrics.SearchSpeed.Name)
                    .AddMeter(Metrics.EmptySearchCount.Name),
                tracing => tracing
                    .AddSource(ActivitySources.IndexApp.Name)
                    .AddSource(ActivitySources.SearchApp.Name));

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
                .UseElasticSearch();

            services
                .AddScoped<IIndexApp, IndexApp>()
                .AddScoped<ISearchApp, SearchApp>()
                .AddScoped<IFeedApp, FeedApp>();

            services
                .AddScoped<IEntryIndexPolicy, PreventDuplicateEntryIndexingPolicy>();

            services
                .Decorate<IIndexApp, IndexAppLogDecorator>()
                .Decorate<IIndexApp, IndexAppMetricsDecorator>()
                .Decorate<IIndexApp, IndexAppEventPublisherDecorator>();

            services
                .Decorate<ISearchApp, SearchAppSearchSpeedMetricsDecorator>()
                .Decorate<ISearchApp, SearchAppEmptySearchMetricsDecorator>()
                .Decorate<ISearchApp, SearchAppSearchCountMetricsDecorator>()
                .Decorate<ISearchApp, SearchAppLogDecorator>();

            services
                .AddScoped<IEventFactory, EventFactory>()
                .AddScoped<IIndexedDocumentPublisher, DaprIndexedDocumentPublisher>();
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
