using AutoMapper;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;
using DocumentSearchResponse = MattCanello.NewsFeed.SearchApi.Domain.Responses.SearchResponse<MattCanello.NewsFeed.SearchApi.Domain.Models.Document>;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Repositories
{
    public sealed class ElasticSearchDocumentSearchRepository : IDocumentSearchRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly IIndexNameBuilder _indexNameBuilder;
        private readonly IMapper _mapper;

        public ElasticSearchDocumentSearchRepository(IElasticClient elasticClient, IIndexNameBuilder indexNameBuilder, IMapper mapper)
        {
            _elasticClient = elasticClient;
            _indexNameBuilder = indexNameBuilder;
            _mapper = mapper;
        }

        public async Task<DocumentSearchResponse> SearchAsync(string? query = null, Paging? paging = null, string? feedId = null, CancellationToken cancellationToken = default)
        {
            var indexName = GetIndexName(feedId)!;

            paging ??= new Paging();

            var response = await _elasticClient.SearchTemplateAsync<ElasticSearch.Models.Entry>(new SearchTemplateRequest(indexName)
            {
                Id = "entries-search",
                Params = new Dictionary<string, object>()
                {
                    { "query_string", query! },
                    { "from", paging.Skip },
                    { "size", paging.Size },
                }
            }, cancellationToken);

            if (!response.IsValid)
            {
                throw new ElasticSearchException(
                   indexName.Name,
                   response.ServerError?.Error?.Reason,
                   response.OriginalException);
            }

            var entries = response.Hits
                .Select(hit => new Document(hit.Id, hit.Source.FeedId!, _mapper.Map<Entry>(hit.Source)))
                .ToList();

            return new DocumentSearchResponse()
            {
                Paging = paging,
                Total = response.Total,
                Results = entries
            };
        }

        private IndexName? GetIndexName(string? feedId = null)
            => (!string.IsNullOrEmpty(feedId))
                ? _indexNameBuilder.WithFeedId(feedId).Build()
                : _indexNameBuilder.AllEntriesIndices().Build();
    }
}
