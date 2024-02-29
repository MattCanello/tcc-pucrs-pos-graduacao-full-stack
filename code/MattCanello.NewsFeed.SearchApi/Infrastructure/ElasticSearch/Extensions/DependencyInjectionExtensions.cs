using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Extensions
{
    static class DependencyInjectionExtensions
    {
        public static void UseElasticSearch(this IServiceCollection services)
        {
            services
                .AddAutoMapper((config) => config.AddProfile<ElasticSearchModelProfile>());

            services
                .AddSingleton<IElasticSearchExceptionFactory, ElasticSearchExceptionFactory>()
                .AddScoped<IIndexService, ElasticSearchIndexService>();

            services
                .AddSingleton<IElasticModelFactory, ElasticModelFactory>();

            services
                .AddSingleton<IElasticClient>((sp) =>
                {
                    var elasticUrl = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL");

                    if (!string.IsNullOrEmpty(elasticUrl))
                        return new ElasticClient(new Uri(elasticUrl, UriKind.Absolute));

                    return new ElasticClient();
                });
        }
    }
}
