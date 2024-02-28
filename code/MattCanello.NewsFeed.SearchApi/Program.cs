using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.SearchApi
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMapperProfiles();
            builder.Services.AddAppServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
                .AddScoped<IIndexService, ElasticSearchIndexService>();
        }

        private static void AddMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper((config) =>
            {
                config.AddProfile<ElasticSearchModelProfile>();
            });
        }
    }
}
