using AutoMapper;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;
using DocumentSearchResponse = MattCanello.NewsFeed.SearchApi.Domain.Responses.SearchResponse<MattCanello.NewsFeed.SearchApi.Domain.Models.Document>;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Application
{
    public sealed class ElasticSearchSearchApp : ISearchApp
    {
        private readonly IElasticClient _elasticClient;
        private readonly IIndexNameBuilder _indexNameBuilder;
        private readonly IQueryStringProcessor _queryStringProcessor;
        private readonly IMapper _mapper;

        public ElasticSearchSearchApp(IElasticClient elasticClient, IIndexNameBuilder indexNameBuilder, IQueryStringProcessor queryStringProcessor, IMapper mapper)
        {
            _elasticClient = elasticClient;
            _indexNameBuilder = indexNameBuilder;
            _queryStringProcessor = queryStringProcessor;
            _mapper = mapper;
        }

        public async Task<DocumentSearchResponse> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(searchCommand);

            searchCommand.Paging ??= new Paging();

            var indexName = GetIndexName(searchCommand);

            var result = await _elasticClient.SearchTemplateAsync<ElasticSearch.Models.Entry>(new SearchTemplateRequest(indexName)
            {
                Id = "entries-search",
                Params = new Dictionary<string, object>()
                {
                    { "query_string", searchCommand.Query! },
                    { "from", searchCommand.Paging.Skip },
                    { "size", searchCommand.Paging.Size },
                }
            }, cancellationToken);

            var entries = result.Hits
                .Select(hit => new Document(hit.Id, hit.Source.FeedId!, _mapper.Map<Entry>(hit.Source)))
                .ToList();

            return new DocumentSearchResponse()
            {
                Paging = searchCommand.Paging,
                Total = result.Total,
                Results = entries
            };
        }

        private IndexName? GetIndexName(SearchCommand searchCommand)
            => (!string.IsNullOrEmpty(searchCommand.FeedId))
                ? _indexNameBuilder.WithFeedId(searchCommand.FeedId).Build()
                : _indexNameBuilder.AllEntriesIndices().Build();
    }
}
