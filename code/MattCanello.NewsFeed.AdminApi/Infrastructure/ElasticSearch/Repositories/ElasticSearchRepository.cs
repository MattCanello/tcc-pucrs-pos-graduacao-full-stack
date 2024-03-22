using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.Cross.ElasticSearch.Exceptions;
using MattCanello.NewsFeed.Cross.ElasticSearch.Extensions;
using Nest;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Repositories
{
    public sealed class ElasticSearchRepository : IElasticSearchRepository, IElasticSearchManagementRepository
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchRepository(IElasticClient elasticClient)
            => _elasticClient = elasticClient;

        public async Task EnsureIndexExistsAsync(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> selector, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(indexName);

            var existsResponse = await _elasticClient.Indices.ExistsAsync(indexName, ct: cancellationToken);

            if (existsResponse.Exists)
                return;

            var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName, selector, cancellationToken);

            if (createIndexResponse.IsValid)
                return;

            throw new ElasticSearchException(
                indexName,
                createIndexResponse.ServerError?.Error?.Reason,
                createIndexResponse.OriginalException);
        }

        public async Task IndexAsync<TModel>(string docId, TModel document, string indexName, CancellationToken cancellationToken = default)
            where TModel : class, new()
        {
            ArgumentNullException.ThrowIfNull(docId);
            ArgumentNullException.ThrowIfNull(document);
            ArgumentNullException.ThrowIfNull(indexName);

            var indexResponse = await _elasticClient.IndexAsync(document, (ix) =>
                ix.Id(docId).Index(indexName), cancellationToken);

            if (indexResponse.IsValid)
                return;

            throw new ElasticSearchException(
                indexName,
                indexResponse.ServerError?.Error?.Reason,
                indexResponse.OriginalException);
        }

        public async Task<TModel?> GetByIdAsync<TModel>(string docId, string indexName, CancellationToken cancellationToken = default)
            where TModel : class, new()
        {
            ArgumentNullException.ThrowIfNull(docId);
            ArgumentNullException.ThrowIfNull(indexName);

            var getResponse = await _elasticClient
                .GetAsync(new DocumentPath<TModel>(docId), (get) => get.Index(indexName), cancellationToken);

            var isIndexNotFound = getResponse.ServerError.IsIndexNotFound();

            var isEntryNotFound = getResponse.IsDocumentNotFound();

            if (isIndexNotFound || isEntryNotFound)
                return null;

            if (getResponse.Found && getResponse.Source != null)
                return getResponse.Source;

            throw new ElasticSearchException(
                indexName,
                getResponse.ServerError?.Error?.Reason,
                getResponse.OriginalException);
        }
    }
}
