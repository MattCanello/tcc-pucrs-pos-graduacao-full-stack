using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Domain.Models;

namespace MattCanello.NewsFeed.CronApi.Tests.Fakes
{
    public sealed class FakeFeedRepository : IFeedRepository
    {
        private readonly IDictionary<byte, IDictionary<string, Feed>> _feeds;
        public FakeFeedRepository(IDictionary<byte, IDictionary<string, Feed>> feeds)
        {
            _feeds = feeds;
        }

        public Task<Feed?> GetFeedAsync(byte slot, string feedId, CancellationToken cancellationToken = default)
        {
            if (!_feeds.TryGetValue(slot, out var feeds))
                return Task.FromResult<Feed?>(null);

            if (!feeds.TryGetValue(feedId, out var feed))
                return Task.FromResult<Feed?>(null);

            return Task.FromResult<Feed?>(feed);
        }

        public Task<IReadOnlySet<string>> GetFeedIdsAsync(byte slot, CancellationToken cancellationToken = default)
        {
            if (!_feeds.TryGetValue(slot, out var feeds))
                return Task.FromResult<IReadOnlySet<string>>(new HashSet<string>(capacity: 0));

            return Task.FromResult<IReadOnlySet<string>>(new HashSet<string>(feeds.Keys, StringComparer.OrdinalIgnoreCase));
        }

        public Task PlaceFeedAsync(byte slot, Feed feed, CancellationToken cancellationToken = default)
        {
            if (!_feeds.ContainsKey(slot))
                _feeds[slot] = new Dictionary<string, Feed>(capacity: 1);

            _feeds[slot].Add(feed.FeedId, feed);
            return Task.CompletedTask;
        }
    }
}
