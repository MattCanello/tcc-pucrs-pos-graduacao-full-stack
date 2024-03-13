using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IEntryIndexPolicy
    {
        Task EvaluateAsync(Entry entry, IndexName? indexName, CancellationToken cancellationToken = default);
    }
}
