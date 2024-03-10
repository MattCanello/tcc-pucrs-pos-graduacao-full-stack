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
        private readonly IMapper _mapper;

        public ElasticSearchSearchApp(IElasticClient elasticClient, IIndexNameBuilder indexNameBuilder, IMapper mapper)
        {
            _elasticClient = elasticClient;
            _indexNameBuilder = indexNameBuilder;
            _mapper = mapper;
        }

        public async Task<DocumentSearchResponse> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(searchCommand);

            searchCommand.Paging ??= new Paging();

            var indexName = GetIndexName(searchCommand);

            var queryString = string.IsNullOrEmpty(searchCommand.Query)
                ? "*"
                : $"*{searchCommand.Query?.Replace("*", "")?.Replace("?", "")}*";

            var result = await _elasticClient.SearchAsync<ElasticSearch.Models.Entry>(s => s
                .Index(indexName)
                .Skip(searchCommand.Paging.Skip)
                .Take(searchCommand.Paging.Size)
                .Query(q => q.QueryString(x => x.Query(queryString)))
                .Sort(sort => sort.Descending(field => field.PublishDate))
                , cancellationToken);

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
