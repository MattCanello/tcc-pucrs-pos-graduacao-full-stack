using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services
{
    public sealed class ElasticSearchIndexApp : IIndexApp
    {
        private readonly IElasticSearchRepository<Entry> _elasticSearchRepository;
        private readonly IElasticModelFactory _elasticModelFactory;
        private readonly IIndexNameBuilder _indexNameBuilder;
        private readonly IEntryIndexPolicy _entryIndexPolicy;

        public ElasticSearchIndexApp(
            IElasticSearchRepository<Entry> elasticSearchRepository, 
            IElasticModelFactory elasticModelFactory, 
            IIndexNameBuilder indexNameBuilder,
            IEntryIndexPolicy entryIndexPolicy)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _elasticModelFactory = elasticModelFactory;
            _indexNameBuilder = indexNameBuilder;
            _entryIndexPolicy = entryIndexPolicy;
        }

        public async Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var elasticModel = _elasticModelFactory.CreateElasticModel(command);

            var indexName = _indexNameBuilder
                .WithFeedId(command.FeedId!)
                .Build();

            await _entryIndexPolicy.EvaluateAsync(elasticModel, indexName, cancellationToken);

            return await _elasticSearchRepository
                .IndexAsync(elasticModel, indexName!, cancellationToken);
        }
    }
}
