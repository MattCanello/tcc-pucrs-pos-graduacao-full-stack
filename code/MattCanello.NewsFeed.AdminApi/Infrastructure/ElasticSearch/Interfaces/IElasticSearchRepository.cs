using MattCanello.NewsFeed.AdminApi.Domain.Responses;
using Nest;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticSearchRepository
    {
        Task IndexAsync<TModel>(string docId, TModel document, string indexName, CancellationToken cancellationToken = default) where TModel : class, new();
        Task<TModel?> GetByIdAsync<TModel>(string docId, string indexName, CancellationToken cancellationToken = default) where TModel : class, new();
        Task<QueryResponse<TModel>> QueryAsync<TModel>(int pageSize, int skip, string indexName, Func<SortDescriptor<TModel>, IPromise<IList<ISort>>>? sortSelector = null, CancellationToken cancellationToken = default) where TModel: class, new();
    }
}
