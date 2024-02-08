using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Repositories
{
    public interface IFeedRepository
    {
        Task<Feed?> GetAsync(string feedId, CancellationToken cancellationToken = default);

        Task UpdateAsync(string feedId, Feed feed, CancellationToken cancellationToken = default);

        Task DeleteAsync(string feedId, CancellationToken cancellationToken = default);
    }
}