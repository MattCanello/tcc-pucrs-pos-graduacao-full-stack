using MattCanello.NewsFeed.Frontend.Server.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Interfaces
{
    public interface IFeedRepository
    {
        Task<Feed> GetFeedAsync(string feedId, CancellationToken cancellationToken = default);
        Task<(Feed, Channel)> GetFeedAndChannelAsync(string feedId, CancellationToken cancellationToken = default);
    }
}
