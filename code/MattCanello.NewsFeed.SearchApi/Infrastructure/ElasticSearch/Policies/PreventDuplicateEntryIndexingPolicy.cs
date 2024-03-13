using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Policies
{
    public sealed class PreventDuplicateEntryIndexingPolicy : IEntryIndexPolicy
    {
        private readonly IElasticSearchRepository<Entry> _elasticSearchRepository;

        public PreventDuplicateEntryIndexingPolicy(IElasticSearchRepository<Entry> elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public async Task EvaluateAsync(Entry entry, IndexName? indexName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(indexName);

            var findResult = await _elasticSearchRepository.FindAsync<string?>(entry => entry.EntryId, entry.EntryId, indexName, cancellationToken);

            if (findResult.IsNotFound)
                return;

            throw new IndexPolicyException(indexName.Name, "The provided entry was already indexed previously.");
        }
    }
}
