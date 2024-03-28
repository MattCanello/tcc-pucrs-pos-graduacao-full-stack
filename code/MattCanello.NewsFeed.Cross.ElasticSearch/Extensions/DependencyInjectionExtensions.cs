using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace MattCanello.NewsFeed.Cross.ElasticSearch.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services)
        {
            services
                .AddSingleton<IElasticClient>((sp) =>
                {
                    var elasticUrl = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL");

                    if (!string.IsNullOrEmpty(elasticUrl))
                        return new ElasticClient(new Uri(elasticUrl, UriKind.Absolute));

                    var cloudId = Environment.GetEnvironmentVariable("ELASTICSEARCH_CLOUDID");
                    var apiKey = Environment.GetEnvironmentVariable("ELASTICSEARCH_API_KEY");

                    if (!string.IsNullOrEmpty(cloudId))
                        return new ElasticClient(cloudId, new ApiKeyAuthenticationCredentials(apiKey));

                    throw new InvalidOperationException("Nenhuma configuração de ElasticSearch localizada.");
                });

            return services;
        }
    }
}
