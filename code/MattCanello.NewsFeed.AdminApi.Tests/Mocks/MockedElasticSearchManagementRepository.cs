using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedElasticSearchManagementRepository : IElasticSearchManagementRepository
    {
        private readonly Action<string, Func<CreateIndexDescriptor, ICreateIndexRequest>> _ensureIndexExistsAction;

        public MockedElasticSearchManagementRepository(Action<string, Func<CreateIndexDescriptor, ICreateIndexRequest>> ensureIndexExistsAction) 
            => _ensureIndexExistsAction = ensureIndexExistsAction ?? throw new ArgumentNullException(nameof(ensureIndexExistsAction));

        public Task EnsureIndexExistsAsync(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> selector, CancellationToken cancellationToken = default)
        {
            _ensureIndexExistsAction(indexName, selector);

            return Task.CompletedTask;
        }
    }
}
