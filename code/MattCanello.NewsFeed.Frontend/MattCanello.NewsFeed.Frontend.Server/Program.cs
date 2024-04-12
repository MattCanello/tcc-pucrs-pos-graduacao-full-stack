using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Extensions;
using MattCanello.NewsFeed.Frontend.Server.Domain.Application;
using MattCanello.NewsFeed.Frontend.Server.Domain.Factories;
using MattCanello.NewsFeed.Frontend.Server.Domain.Handlers;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Caching;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Clients;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Configuration;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.EventPublishers;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Filters;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Hubs;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Profiles;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Repositories;
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
            builder.Services.AddAppServices(builder.Configuration);
            builder.Services.ConfigureHealthChecks();
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(EnvironmentVariables.FrontendBaseUrl())
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                    });
            });

            builder.Services.AddResponseCaching();

            builder.Services.AddResponseCompression();

            builder.AddDefaultTelemetry();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

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

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
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
                .AddScoped<INewEntryHandler, PublishNewEntryHandler>()
                .Decorate<INewEntryHandler, UpdateHomePageCacheNewEntryHandler>();

            services
                .AddScoped<HomePageCacheFilter>();

            services
                .AddScoped<IHomePageCachingService, InMemoryHomePageCachingService>();

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
