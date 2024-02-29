using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Tests.Mocks
{
    internal sealed class FailMockedElasticSearchRepository : IElasticSearchRepository
    {
        private readonly Func<IndexException> _indexExceptionFactory;

        public FailMockedElasticSearchRepository(Func<IndexException>? indexExceptionFactory = null)
        {
            _indexExceptionFactory = indexExceptionFactory ?? new Func<IndexException>(() => new IndexException());
        }

        public Task<string> IndexAsync<TElasticModel>(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default)
            where TElasticModel : class, new()
        {
            return Task.FromException<string>(_indexExceptionFactory());
        }
    }
}
