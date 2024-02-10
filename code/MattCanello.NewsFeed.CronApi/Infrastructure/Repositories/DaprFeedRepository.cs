using Dapr.Client;
using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Domain.Models;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Repositories
{
    public sealed class DaprFeedRepository : IFeedRepository
    {
        const string StoreName = "cronstatestore";
        private readonly DaprClient _daprClient;

        public DaprFeedRepository(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<Feed?> GetFeedAsync(byte slot, string feedId, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);
            ArgumentNullException.ThrowIfNull(feedId);

            var key = GetFeedKey(slot, feedId);

            var feed = await _daprClient.GetStateAsync<Feed>(StoreName, key, cancellationToken: cancellationToken);

            return feed;
        }

        public async Task<IReadOnlySet<string>> GetFeedIdsAsync(byte slot, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            var key = GetSlotKey(slot);

            var feedIds = await _daprClient.GetStateAsync<HashSet<string>>(StoreName, key, cancellationToken: cancellationToken);

            return feedIds ?? new HashSet<string>(capacity: 0);
        }

        public async Task PlaceFeedAsync(byte slot, Feed feed, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);
            ArgumentNullException.ThrowIfNull(feed);

            var key = GetFeedKey(slot, feed.FeedId);

            var feedStateTask = _daprClient.SaveStateAsync(StoreName, key, feed, cancellationToken: cancellationToken);
            var feedIdTask = PlaceFeedIdAsync(slot, feed.FeedId, cancellationToken);

            await Task.WhenAll(feedStateTask, feedIdTask);
        }

        private async Task PlaceFeedIdAsync(byte slot, string feedId, CancellationToken cancellationToken = default)
        {
            var feedIds = await GetFeedIdsAsync(slot, cancellationToken);

            if (feedIds.Contains(feedId))
                return;

            var newFeedIds = new HashSet<string>(feedIds, StringComparer.OrdinalIgnoreCase)
            {
                feedId
            };

            var slotKey = GetSlotKey(slot);

            await _daprClient.SaveStateAsync(StoreName, slotKey, newFeedIds, cancellationToken: cancellationToken);
        }

        private static string GetSlotKey(byte slot) => $"s{slot:00}";
        private static string GetFeedKey(byte slot, string feedId) => $"s{slot:00}/{feedId}";

        public async Task UpdateLastExecutionDateAsync(byte slot, string feedId, DateTimeOffset lastUpdateDate, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);
            ArgumentNullException.ThrowIfNull(feedId);

            var feed = await GetFeedAsync(slot, feedId, cancellationToken);

            if (feed is null)
                return;

            feed.LastExecutionDate = lastUpdateDate;

            var feedKey = GetFeedKey(slot, feedId);

            await _daprClient.SaveStateAsync(StoreName, feedKey, feed, cancellationToken: cancellationToken);
        }
    }
}
