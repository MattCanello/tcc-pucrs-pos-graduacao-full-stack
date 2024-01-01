using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IFeedRepository
    {
        Task<Feed> GetAsync(string feedId, CancellationToken cancellationToken = default);

        Task UpdateAsync(string feedId, Feed feed, CancellationToken cancellationToken = default);
    }
}