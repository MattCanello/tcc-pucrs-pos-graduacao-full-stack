using MattCanello.NewsFeed.Frontend.Server.Domain.Commands;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface INewEntryHandler
    {
        Task HandleAsync(NewEntryFoundCommand command, CancellationToken cancellationToken = default);
    }
}
