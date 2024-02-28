using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Services
{
    public sealed class ElasticSearchIndexService : IIndexService
    {
        public Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
