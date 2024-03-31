using AutoMapper;
using MattCanello.NewsFeed.Cross.ElasticSearch.Exceptions;
using MattCanello.NewsFeed.Cross.ElasticSearch.Extensions;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
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

            return await SearchIndexAsync(indexName, query, paging, cancellationToken);
        }

        public async Task<DocumentSearchResponse> SearchByChannelAsync(string? query = null, Paging? paging = null, string? channelId = null, CancellationToken cancellationToken = default)
        {
            var indexName = GetIndexName(channelId: channelId)!;

            return await SearchIndexAsync(indexName, query, paging, cancellationToken);
        }

        private async Task<DocumentSearchResponse> SearchIndexAsync(IndexName indexName, string ? query = null, Paging? paging = null, CancellationToken cancellationToken = default)
        {
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

            return ProcessSearchResponse(paging, indexName, response);
        }

        private IndexName? GetIndexName(string? feedId = null)
            => (!string.IsNullOrEmpty(feedId))
                ? _indexNameBuilder.WithFeedId(feedId).Build()
                : _indexNameBuilder.AllEntriesIndices().Build();

        private IndexName? GetIndexName(string? feedId = null, string? channelId = null)
        {
            if (!string.IsNullOrEmpty(feedId))
                return  GetIndexName(feedId)!;
            else if (!string.IsNullOrEmpty(channelId))
                return GetIndexName($"{channelId}-*")!;
            else
                return GetIndexName(null);
        }

        public async Task<FindResponse<Document>> FindByIdAsync(string entryId, string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entryId);
            ArgumentNullException.ThrowIfNull(feedId);

            var indexName = GetIndexName(feedId)!;

            var response = await _elasticClient.SearchAsync<ElasticSearch.Models.Entry>((queryBuilder) => queryBuilder
                .Index(indexName)
                .Size(1)
                .Query(q => q.Term(term => term.Value(entryId).Field(entry => entry.EntryId))), cancellationToken);

            var hit = response.Hits?.FirstOrDefault();

            if (hit != null)
                return new FindResponse<Document>(
                    hit.Id, 
                    new Document(hit.Id, hit.Source.FeedId!, _mapper.Map<Entry>(hit.Source)),
                    hit.Index);

            if (response.IsValid || response.IsIndexNotFound())
                return FindResponse<Document>.NotFound;

            throw new ElasticSearchException(
               indexName.Name,
               response.ServerError?.Error?.Reason,
               response.OriginalException);
        }

        public async Task<DocumentSearchResponse> GetRecentAsync(Paging paging, string? feedId = null, string? channelId = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(paging);

            var indexName = GetIndexName(feedId, channelId)!;

            var response = await _elasticClient.SearchAsync<Models.Entry>((queryBuilder) => queryBuilder
                .Index(indexName)
                .Size(paging.Size)
                .Skip(paging.Skip)
                .Sort(q => q.Descending(t => t.PublishDate)), 
                cancellationToken);

            return ProcessSearchResponse(paging, indexName, response);
        }

        private DocumentSearchResponse ProcessSearchResponse(Paging paging, IndexName indexName, ISearchResponse<ElasticSearch.Models.Entry> response)
        {
            if (response.IsIndexNotFound())
                return DocumentSearchResponse.CreateEmpty(paging);

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

            return new DocumentSearchResponse(
                entries,
                response.Total,
                paging);
        }
    }
}
