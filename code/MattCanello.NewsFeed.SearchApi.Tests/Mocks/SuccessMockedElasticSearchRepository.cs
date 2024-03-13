using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Responses;
using Nest;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace MattCanello.NewsFeed.SearchApi.Tests.Mocks
{
    internal sealed class SuccessMockedElasticSearchRepository<TElasticModel> : IElasticSearchRepository<TElasticModel>
        where TElasticModel : class, new()
    {
        private readonly IDictionary<Key, TElasticModel> _data = new ConcurrentDictionary<Key, TElasticModel>();

        public int Count => _data.Count;

        public string? LastProducedId { get; private set; }

        public Task<string> IndexAsync(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default)
        {
            var id = DateTimeOffset.Now.Ticks.ToString("F0");

            var key = new Key(indexName.Name, id);

            _data.TryAdd(key, elasticModel);

            LastProducedId = id;

            return Task.FromResult(id);
        }

        public Task<TElasticModel> GetAsync(IndexName indexName, string id, CancellationToken cancellationToken = default) 
        {
            var key = new Key(indexName.Name, id);

            if (_data.TryGetValue(key, out var elasticModel))
                return Task.FromResult((TElasticModel)Convert.ChangeType(elasticModel, typeof(TElasticModel)));

            throw new DocumentNotFoundException(id);
        }

        public Task<FindResponse<TElasticModel>> FindAsync<TValue>(Expression<Func<TElasticModel, TValue>> fieldSelector, TValue value, IndexName indexName, CancellationToken cancellationToken = default) 
        {
            var fieldFunction = fieldSelector.Compile();

            var item = _data
                .Where(kvp => kvp.Key.IndexName == indexName.Name)
                .FirstOrDefault(kvp => fieldFunction(kvp.Value)?.Equals(value) ?? false);

            if (item.Value is null)
                return Task.FromResult(FindResponse<TElasticModel>.NotFound);

            return Task.FromResult(new FindResponse<TElasticModel>(item.Key.Id, item.Value));
        }

        private sealed record Key(string IndexName, string Id);
    }
}
