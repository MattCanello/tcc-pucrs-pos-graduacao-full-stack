using Elasticsearch.Net;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Repositories
{
    public sealed class ElasticSearchRepository<TElasticModel> : IElasticSearchRepository<TElasticModel>
        where TElasticModel : class, new()
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<string> IndexAsync(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default)
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
                indexName.Name,
                indexResponse.ServerError?.Error?.Reason,
                indexResponse.OriginalException);
        }

        public async Task<TElasticModel> GetAsync(IndexName indexName, string id, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(indexName);
            ArgumentNullException.ThrowIfNull(id);

            var request = new GetRequest<TElasticModel>(indexName, id);

            var getDocumentResponse = await _elasticClient.GetAsync<TElasticModel>(request, cancellationToken);

            var isIndexNotFound = IsIndexNotFound(getDocumentResponse.ServerError);

            var isEntryNotFound = (getDocumentResponse.ApiCall?.HttpStatusCode == 404)
                || (getDocumentResponse.ServerError is null && !getDocumentResponse.Found)
                || (getDocumentResponse.Found && getDocumentResponse.Source is null);

            if (isIndexNotFound || isEntryNotFound)
                throw new DocumentNotFoundException(id);

            if (getDocumentResponse.Found && getDocumentResponse.Source != null)
                return getDocumentResponse.Source;

            throw new ElasticSearchException(
                indexName.Name,
                getDocumentResponse.ServerError?.Error?.Reason,
                getDocumentResponse.OriginalException);
        }

        private static bool IsIndexNotFound(ServerError? serverError)
            => serverError?.Status == 404;
    }
}
