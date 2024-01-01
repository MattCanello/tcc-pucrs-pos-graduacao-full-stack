using MattCanello.NewsFeed.RssReader.Factories;
using MattCanello.NewsFeed.RssReader.Infrastructure;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Services;

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
                // TODO: Substituir pela implementação de fato.
                .AddSingleton<IFeedRepository, InMemoryFeedRepository>();

            services
                .AddSingleton<ReadRssRequestMessageFactory>();

            services
                .AddHttpClient()
                .AddScoped<IRssClient, RssClient>()
                .AddScoped<IRssService, RssService>();
        }
    }
}
