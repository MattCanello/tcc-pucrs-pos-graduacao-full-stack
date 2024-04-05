using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Caching
{
    public sealed class MemoryCachingService : ICachingService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCachingService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task SetAsync<TModel>(string key, TModel data, TimeSpan? expiry = null, CancellationToken cancellationToken = default) 
            where TModel : class, new()
        {
            if (expiry is null)
                _memoryCache.Set(key, data);
            else
                _memoryCache.Set(key, data, expiry.Value);

            return Task.CompletedTask;
        }

        public Task<TModel?> TryGetAsync<TModel>(string key, CancellationToken cancellationToken = default) 
            where TModel : class, new()
        {
            if (_memoryCache.TryGetValue(key, out var data) && data is TModel model)
                return Task.FromResult<TModel?>(model);

            return Task.FromResult<TModel?>(null);
        }
    }
}
