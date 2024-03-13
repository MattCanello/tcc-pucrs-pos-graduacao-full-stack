using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Responses;
using Nest;
using System.Linq.Expressions;

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

        public async Task<FindResponse<TElasticModel>> FindAsync<TValue>(
            Expression<Func<TElasticModel, TValue>> fieldSelector, TValue value, IndexName indexName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(indexName);

            var result = await _elasticClient.SearchAsync<TElasticModel>((queryBuilder) => queryBuilder
                .Index(indexName)
                .Size(1)
                .Query(q => q.Term(term => term.Value(value).Field(fieldSelector))), cancellationToken);

            var hit = result.Hits?.FirstOrDefault();

            if (hit != null)
                return new FindResponse<TElasticModel>(hit.Id, hit.Source);

            if (result.IsValid)
                return FindResponse<TElasticModel>.NotFound;

            throw new ElasticSearchException(
               indexName.Name,
               result.ServerError?.Error?.Reason,
               result.OriginalException);
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

            var isIndexNotFound = getDocumentResponse.ServerError?.Status == 404;

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
    }
}
