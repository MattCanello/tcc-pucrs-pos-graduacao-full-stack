using AutoMapper;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Policies
{
    public sealed class PreventDuplicateEntryIndexingPolicy : IEntryIndexPolicy
    {
        private readonly IElasticSearchRepository<Models.Entry> _elasticSearchRepository;
        private readonly IMapper _mapper;

        public PreventDuplicateEntryIndexingPolicy(IElasticSearchRepository<Models.Entry> elasticSearchRepository, IMapper mapper)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _mapper = mapper;
        }

        public async Task EvaluateAsync(Models.Entry entry, IndexName? indexName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(indexName);

            var findResult = await _elasticSearchRepository.FindAsync(entry => entry.EntryId, entry.EntryId, indexName, cancellationToken);

            if (findResult.IsNotFound)
                return;

            var doc = new Document(findResult.Id, entry.FeedId!, _mapper.Map<Domain.Models.Entry>(entry));

            throw new EntryAlreadyIndexedException(doc, indexName.Name);
        }
    }
}
