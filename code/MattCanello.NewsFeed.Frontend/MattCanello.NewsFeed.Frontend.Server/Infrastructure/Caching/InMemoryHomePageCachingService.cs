using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Caching
{
    // TODO: Usar StateStore do Dapr
    sealed class InMemoryHomePageCachingService : IHomePageCachingService
    {
        private readonly ICachingService _cachingService;
        const string CacheKey = "HOMEPAGECACHE";

        public InMemoryHomePageCachingService(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }

        public async Task<DateTimeOffset?> GetLatestArticleDateAsync(CancellationToken cancellationToken = default)
        {
            var cacheDate = await _cachingService.TryGetAsync<HomePageCacheData>(CacheKey, cancellationToken);

            if (cacheDate is null)
                return null;

            return cacheDate.LastPublishDate;
        }

        public async Task SetLastestArticleDateAsync(DateTimeOffset publishDate, CancellationToken cancellationToken = default)
        {
            var cacheData = await _cachingService.TryGetAsync<HomePageCacheData>(CacheKey, cancellationToken);

            cacheData ??= new HomePageCacheData(publishDate);

            if (publishDate < cacheData.LastPublishDate)
                return;

            cacheData = cacheData with { LastPublishDate = publishDate };

            await _cachingService.SetAsync(CacheKey, cacheData, expiry: null, cancellationToken);
        }

        [Serializable]
        private record class HomePageCacheData
        {
            public HomePageCacheData() { }

            public HomePageCacheData(DateTimeOffset lastPublishDate)
            {
                LastPublishDate = lastPublishDate;
            }

            public DateTimeOffset LastPublishDate { get; init; }
        }
    }
}
