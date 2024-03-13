using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Tests.Mocks
{
    sealed class NoEntryIndexPolicy : IEntryIndexPolicy
    {
        public Task EvaluateAsync(Entry entry, IndexName? indexName, CancellationToken cancellationToken = default) 
            => Task.CompletedTask;
    }
}
