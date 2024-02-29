using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Repositories
{
    public sealed class ElasticSearchRepository : IElasticSearchRepository
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<string> IndexAsync<TElasticModel>(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default)
            where TElasticModel : class, new()
        {
            ArgumentNullException.ThrowIfNull(elasticModel);
            ArgumentNullException.ThrowIfNull(indexName);

            var indexRequest = new IndexRequest<TElasticModel>(index: indexName)
            {
                Document = elasticModel
            };

            var indexResponse = await _elasticClient.IndexAsync(indexRequest, cancellationToken);

            if (indexResponse.IsValid)
                return indexResponse.Id;

            throw new IndexException(
                indexResponse.ServerError?.Error?.Reason,
                indexResponse.OriginalException);
        }
    }
}
