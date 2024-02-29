using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services
{
    public sealed class ElasticSearchIndexService : IIndexService
    {
        private readonly IElasticClient _elasticClient;
        private readonly IElasticModelFactory _elasticModelFactory;
        private readonly IIndexNameBuilder _indexNameBuilder;

        public ElasticSearchIndexService(IElasticClient elasticClient, IElasticModelFactory elasticModelFactory, IIndexNameBuilder indexNameBuilder)
        {
            _elasticClient = elasticClient;
            _elasticModelFactory = elasticModelFactory;
            _indexNameBuilder = indexNameBuilder;
        }

        public async Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var elasticModel = _elasticModelFactory.CreateElasticModel(command);

            var indexName = _indexNameBuilder
                .WithFeedId(command.FeedId!)
                .Build();

            var indexResponse = await _elasticClient.IndexAsync(new IndexRequest<ElasticSearch.Models.Entry>(index: indexName)
            {
                Document = elasticModel
            }, cancellationToken);

            if (indexResponse.IsValid)
                return indexResponse.Id;

            throw new IndexException(
                indexResponse.ServerError?.Error?.Reason,
                indexResponse.OriginalException);
        }
    }
}
