using MattCanello.NewsFeed.RssReader.Factories;
using MattCanello.NewsFeed.RssReader.Infrastructure;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Profiles;
using MattCanello.NewsFeed.RssReader.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.RssReader
{
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

            builder.Services.AddAutoMapper((config) =>
            {
                config.AddProfile<FeedProfile>();
            });

            builder.Services.AddDapr();
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
                .AddScoped<IChannelPublisher, ChannelPublisher>()
                .AddSingleton<IChannelReader, ChannelReader>()
                .AddScoped<IChannelService, ChannelService>();

            services
                .AddScoped<IEntryPublisher, EntryPublisher>()
                .AddSingleton<IEntryReader, EntryReader>()
                .AddScoped<IEntryService, EntryService>();

            services
                .AddSingleton<INonStandardEnricherEvaluator, NonStandardEnricherEvaluator>();

            services
                .AddSingleton<IFeedRepository, DaprFeedRepository>();

            services
                .AddSingleton<ReadRssRequestMessageFactory>();

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
    }
}
