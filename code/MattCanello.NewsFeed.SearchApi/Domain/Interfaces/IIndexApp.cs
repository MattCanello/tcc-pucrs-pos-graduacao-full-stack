using MattCanello.NewsFeed.SearchApi.Domain.Commands;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IIndexApp
    {
        Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default);
    }
}
