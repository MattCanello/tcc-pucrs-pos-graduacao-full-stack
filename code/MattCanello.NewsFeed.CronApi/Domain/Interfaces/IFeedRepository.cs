using MattCanello.NewsFeed.CronApi.Domain.Models;

namespace MattCanello.NewsFeed.CronApi.Domain.Interfaces
{
    public interface IFeedRepository
    {
        Task<IReadOnlySet<string>> GetFeedIdsAsync(byte slot, CancellationToken cancellationToken = default);

        Task<Feed?> GetFeedAsync(byte slot, string feedId, CancellationToken cancellationToken = default);

        Task PlaceFeedAsync(byte slot, Feed feed, CancellationToken cancellationToken = default);

        Task UpdateLastExecutionDateAsync(byte slot, string feedId, DateTimeOffset lastUpdateDate, CancellationToken cancellationToken = default);
    }
}
