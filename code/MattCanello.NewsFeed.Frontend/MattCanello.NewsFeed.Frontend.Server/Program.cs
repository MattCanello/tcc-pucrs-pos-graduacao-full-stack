using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.Telemetry.Extensions;
using MattCanello.NewsFeed.Frontend.Server.Application;
using MattCanello.NewsFeed.Frontend.Server.Clients;
using MattCanello.NewsFeed.Frontend.Server.Configuration;
using MattCanello.NewsFeed.Frontend.Server.Factories;
using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Profiles;
using MattCanello.NewsFeed.Frontend.Server.Repositories;
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

            app.UseAuthorization();

            app.MapControllers();

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
                .AddAutoMapper(config =>
                {
                    config.AddProfile<AdminProfile>();
                    config.AddProfile<SearchProfile>();
                });

            services
                .AddHttpClient<IAdminClient, AdminHttpClient>(options =>
                {
                    options.BaseAddress = new Uri(configuration[EnvironmentVariables.ADMIN_API_BASE_URL]!, UriKind.Absolute);
                    options.DefaultRequestHeaders.Accept.Clear();
                    options.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                });

            services
                .AddHttpClient<ISearchClient, SearchHttpClient>(options =>
                {
                    options.BaseAddress = new Uri(configuration[EnvironmentVariables.SEARCH_API_BASE_URL]!, UriKind.Absolute);
                    options.DefaultRequestHeaders.Accept.Clear();
                    options.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                });
        }

        private static void AddDefaultControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
        }
    }
}
