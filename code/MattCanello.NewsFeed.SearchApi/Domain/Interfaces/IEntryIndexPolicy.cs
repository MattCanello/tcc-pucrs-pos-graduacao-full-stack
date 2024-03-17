using MattCanello.NewsFeed.SearchApi.Domain.Commands;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IEntryIndexPolicy
    {
        Task EvaluateAsync(IndexEntryCommand command, CancellationToken cancellationToken = default);
    }
}
