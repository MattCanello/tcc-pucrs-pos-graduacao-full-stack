using Nest;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticSearchManagementRepository
    {
        Task EnsureIndexExistsAsync(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> selector, CancellationToken cancellationToken = default);
    }
}
