using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Extensions;
using MattCanello.NewsFeed.Frontend.Server.Domain.Application;
using MattCanello.NewsFeed.Frontend.Server.Domain.Factories;
using MattCanello.NewsFeed.Frontend.Server.Domain.Handlers;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Caching;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Clients;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Configuration;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Decorators;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.EventPublishers;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Filters;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Hubs;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Profiles;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Repositories;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.Frontend.Server
{
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
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(EnvironmentVariables.FrontendBaseUrls())
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST", "OPTIONS")
                            .AllowCredentials();

                        builder
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .WithOrigins("*")
                            .AllowAnyHeader()
                            .WithMethods("OPTIONS")
                            .AllowCredentials();
                    });
            });

            builder.Services.AddResponseCaching();

            builder.Services.AddResponseCompression();

            builder.AddDefaultTelemetry(
                metrics => metrics.AddMeter(Metrics.ArticleDetailsHits.Name, Metrics.FrontPageHits.Name, 
                    Metrics.ChannelHits.Name, Metrics.ChannelHits.Name, Metrics.SearchHits.Name,
                    Metrics.NewEntryHits.Name),
                tracing => tracing.AddSource(ActivitySources.ArticleApp.Name, ActivitySources.NewEntryHandler.Name));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapGet("/", () => Results.LocalRedirect("/swagger"));
            }

            app.UseCors();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ArticleHub>("/hubs/article");

            app.MapHealthChecks("/app/health");

            app.Run();
        }

        private static void AddAppServices(this IServiceCollection services)
        {
            services
                .AddScoped<IArticleApp, ArticleApp>();

            services
                .AddSingleton<IArticleDetailsFactory, ArticleDetailsFactory>()
                .AddSingleton<IArticleFactory, ArticleFactory>();

            services
                .AddScoped<IChannelRepository, AdminChannelRepository>()
                .AddScoped<IFeedRepository, AdminFeedRepository>();

            services
                .AddSingleton<IChannelConfiguration, ChannelConfiguration>()
                .AddSingleton<IFrontPageConfiguration, FrontPageConfiguration>();

            services
                .AddMemoryCache()
                .AddSingleton<ICachingConfiguration, CachingConfiguration>()
                .AddScoped<ICachingService, MemoryCachingService>();

            services
                .Decorate<IFeedRepository, CachedFeedRepository>();

            services
                .AddScoped<INewArticlePublisher, NewArticlePublisher>()
                .AddScoped<INewEntryHandler, NewEntryHandler>();

            services
                .Decorate<IArticleApp, ArticleAppMetricsDecorator>()
                .Decorate<IArticleApp, ArticleAppLogsDecorator>();

            services
                .Decorate<INewEntryHandler, NewEntryHandlerMetricsDecorator>()
                .Decorate<INewEntryHandler, NewEntryHandlerLogsDecorator>();

            services
                .AddAutoMapper(config =>
                {
                    config.AddProfile<AdminProfile>();
                    config.AddProfile<SearchProfile>();
                });

            services
                .AddScoped<IAdminClient, AdminDaprHttpClient>();

            services
                .AddScoped<ISearchClient, SearchDaprHttpClient>();
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
    }
}
