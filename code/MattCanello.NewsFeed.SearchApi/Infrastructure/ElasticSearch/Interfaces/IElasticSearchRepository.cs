using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticSearchRepository<TElasticModel>
            where TElasticModel : class, new()
    {
        Task<string> IndexAsync(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default);

        Task<TElasticModel> GetAsync(IndexName indexName, string id, CancellationToken cancellationToken = default);
    }
}