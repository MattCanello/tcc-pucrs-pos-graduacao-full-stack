using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Responses;
using Nest;
using System.Linq.Expressions;

namespace MattCanello.NewsFeed.SearchApi.Tests.Mocks
{
    internal sealed class FailMockedElasticSearchRepository<TElasticModel> : IElasticSearchRepository<TElasticModel>
        where TElasticModel : class, new()
    {
        private readonly Func<IndexException> _indexExceptionFactory;

        public FailMockedElasticSearchRepository(Func<IndexException>? indexExceptionFactory = null)
        {
            _indexExceptionFactory = indexExceptionFactory ?? new Func<IndexException>(() => new IndexException());
        }

        public Task<FindResponse<TElasticModel>> FindAsync<TValue>(Expression<Func<TElasticModel, TValue>> fieldSelector, TValue value, IndexName indexName, CancellationToken cancellationToken = default) 
        {
            throw new NotImplementedException();
        }

        public Task<TElasticModel> GetAsync(IndexName indexName, string id, CancellationToken cancellationToken = default)
        {
            return Task.FromException<TElasticModel>(new DocumentNotFoundException(id));
        }

        public Task<string> IndexAsync(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default)
        {
            return Task.FromException<string>(_indexExceptionFactory());
        }
    }
}
