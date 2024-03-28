using MattCanello.NewsFeed.Cross.ElasticSearch.Extensions;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Builders;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Repositories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services;
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
                .AddScoped<IEntryIndexService, ElasticSearchEntryIndexService>();

            services
                .AddSingleton<IElasticModelFactory, ElasticModelFactory>();

            services
                .AddScoped<IDocumentRepository, ElasticSearchDocumentRepository>()
                .AddScoped<IDocumentSearchRepository, ElasticSearchDocumentSearchRepository>();

            services
                .AddSingleton<IElasticSearchRepository<Entry>, ElasticSearchRepository<Entry>>()
                .AddElasticSearch();
        }
    }
}
