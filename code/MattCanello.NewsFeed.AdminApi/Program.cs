using MattCanello.NewsFeed.AdminApi.Infrastructure.Filters;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;
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
