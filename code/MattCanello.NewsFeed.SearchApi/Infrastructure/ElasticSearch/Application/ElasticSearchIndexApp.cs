using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services
{
    public sealed class ElasticSearchIndexApp : IIndexApp
    {
        private readonly IElasticSearchRepository _elasticSearchRepository;
        private readonly IElasticModelFactory _elasticModelFactory;
        private readonly IIndexNameBuilder _indexNameBuilder;

        public ElasticSearchIndexApp(IElasticSearchRepository elasticSearchRepository, IElasticModelFactory elasticModelFactory, IIndexNameBuilder indexNameBuilder)
        {
            _elasticSearchRepository = elasticSearchRepository;
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

            return await _elasticSearchRepository
                .IndexAsync(elasticModel, indexName!, cancellationToken);
        }
    }
}
