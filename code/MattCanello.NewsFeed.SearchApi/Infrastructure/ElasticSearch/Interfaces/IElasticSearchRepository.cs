using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Responses;
using Nest;
using System.Linq.Expressions;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticSearchRepository<TElasticModel>
            where TElasticModel : class, new()
    {
        Task<FindResponse<TElasticModel>> FindAsync<TValue>(
            Expression<Func<TElasticModel, TValue>> fieldSelector, TValue value, IndexName indexName, CancellationToken cancellationToken = default);

        Task<string> IndexAsync(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default);

        Task<TElasticModel> GetAsync(IndexName indexName, string id, CancellationToken cancellationToken = default);
    }
}