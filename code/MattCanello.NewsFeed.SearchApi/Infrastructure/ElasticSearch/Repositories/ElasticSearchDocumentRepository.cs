using AutoMapper;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Repositories
{
    public sealed class ElasticSearchDocumentRepository : IDocumentRepository
    {
        private readonly IElasticSearchRepository<Models.Entry> _elasticSearchRepository;
        private readonly IIndexNameBuilder _indexNameBuilder;
        private readonly IMapper _mapper;

        public ElasticSearchDocumentRepository(IElasticSearchRepository<Models.Entry> elasticSearchRepository, IIndexNameBuilder indexNameBuilder, IMapper mapper)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _indexNameBuilder = indexNameBuilder;
            _mapper = mapper;
        }

        public async Task<Document> GetByIdAsync(string feedId, string id, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(id);

            var indexName = _indexNameBuilder
                .WithFeedId(feedId)
                .Build();

            var elasticSearchEntry = await _elasticSearchRepository.GetAsync(indexName!, id, cancellationToken);

            var domainEntry = _mapper.Map<Entry>(elasticSearchEntry);

            return new Document(id, feedId, domainEntry);
        }
    }
}
