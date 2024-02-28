using MattCanello.NewsFeed.SearchApi.Domain.Commands;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IIndexService
    {
        Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default);
    }
}
