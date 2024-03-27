using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IFeedRepository
    {
        Task<Feed?> GetByIdAsync(string feedId, CancellationToken cancellationToken = default);

        Task<Feed> CreateAsync(Feed feed, CancellationToken cancellationToken = default);

        Task<Feed> UpdateAsync(Feed feed, CancellationToken cancellationToken = default);
    }
}
