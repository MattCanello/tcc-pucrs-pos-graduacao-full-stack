using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;
using System.Collections.Concurrent;

namespace MattCanello.NewsFeed.SearchApi.Tests.Mocks
{
    internal sealed class SuccessMockedElasticSearchRepository : IElasticSearchRepository
    {
        private readonly IDictionary<Key, object> _data = new ConcurrentDictionary<Key, object>();

        public int Count => _data.Count;

        public string? LastProducedId { get; private set; }

        public Task<string> IndexAsync<TElasticModel>(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default)
            where TElasticModel : class, new()
        {
            var id = DateTimeOffset.Now.Ticks.ToString("F0");

            var key = new Key(indexName.Name, id);

            _data.TryAdd(key, elasticModel);

            LastProducedId = id;

            return Task.FromResult(id);
        }

        public Task<TElasticModel> GetAsync<TElasticModel>(IndexName indexName, string id, CancellationToken cancellationToken = default) 
            where TElasticModel : class, new()
        {
            var key = new Key(indexName.Name, id);

            if (_data.TryGetValue(key, out var elasticModel))
                return Task.FromResult((TElasticModel)Convert.ChangeType(elasticModel, typeof(TElasticModel)));

            throw new DocumentNotFoundException(id);
        }

        private sealed record Key(string IndexName, string Id);
    }
}
