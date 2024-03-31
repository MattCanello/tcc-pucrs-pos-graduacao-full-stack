using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IFeedRepository
    {
        Task<(Feed, Channel)> GetFeedAndChannelAsync(string feedId, CancellationToken cancellationToken = default);
    }
}
