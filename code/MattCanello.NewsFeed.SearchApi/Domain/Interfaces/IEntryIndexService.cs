using MattCanello.NewsFeed.SearchApi.Domain.Commands;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IEntryIndexService
    {
        Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default);
    }
}
