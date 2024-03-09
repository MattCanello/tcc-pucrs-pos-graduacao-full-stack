using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Builders;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Repositories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services;
using Nest;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Extensions
{
    [ExcludeFromCodeCoverage]
    static class DependencyInjectionExtensions
    {
        public static void UseElasticSearch(this IServiceCollection services)
        {
            services
                .AddAutoMapper((config) => config.AddProfile<ElasticSearchModelProfile>());

            services
                .AddScoped<IIndexNameBuilder, IndexNameBuilder>()
                .AddScoped<IIndexApp, ElasticSearchIndexApp>();

            services
                .AddSingleton<IElasticModelFactory, ElasticModelFactory>();

            services
                .AddScoped<IEntryRepository, ElasticSearchEntryRepository>();

            services
                .AddSingleton<IElasticSearchRepository, ElasticSearchRepository>()
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
