using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Caching
{
    public sealed class CachedFeedRepository : IFeedRepository
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ICachingService _cachingService;
        private readonly ICachingConfiguration _cachingConfiguration;

        public CachedFeedRepository(IFeedRepository feedRepository, ICachingService cachingService, ICachingConfiguration cachingConfiguration)
        {
            _feedRepository = feedRepository;
            _cachingService = cachingService;
            _cachingConfiguration = cachingConfiguration;
        }

        public async Task<(Feed, Channel)> GetFeedAndChannelAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var feedCacheKey = GetFeedCacheKey(feedId);
            var channelCacheKey = GetChannelCacheKey(feedId);

            var feedTask = _cachingService.TryGetAsync<Feed>(feedCacheKey, cancellationToken);
            var channelTask = _cachingService.TryGetAsync<Channel>(channelCacheKey, cancellationToken);

            await Task.WhenAll(feedTask, channelTask);

            if (feedTask.Result is null || channelTask.Result is null)
                return await EvaluateAndStoreAsync(feedId, cancellationToken);

            return (feedTask.Result, channelTask.Result);
        }

        private async Task<(Feed, Channel)> EvaluateAndStoreAsync(string feedId, CancellationToken cancellationToken = default)
        {
            var (feed, channel) = await _feedRepository.GetFeedAndChannelAsync(feedId, cancellationToken);

            var setFeedTask = _cachingService.SetAsync(GetFeedCacheKey(feedId), feed, _cachingConfiguration.GetFeedExpiryTime(), cancellationToken);
            var setChannel = _cachingService.SetAsync(GetChannelCacheKey(feedId), channel, _cachingConfiguration.GetChannelExpiryTime(), cancellationToken);

            await Task.WhenAll(setFeedTask, setChannel);

            return (feed, channel);
        }

        public static string GetFeedCacheKey(string feedId)
            => $"FEED_{feedId}";

        public static string GetChannelCacheKey(string feedId)
            => $"CHANNEL_FROMFEEDID_{feedId}";
    }
}