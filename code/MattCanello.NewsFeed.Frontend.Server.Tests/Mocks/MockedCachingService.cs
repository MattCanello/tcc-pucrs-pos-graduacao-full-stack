using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    sealed class MockedCachingService : ICachingService
    {
        private readonly IDictionary<string, object> _data;

        public MockedCachingService(IDictionary<string, object>? data = null) 
            => _data = data ?? new Dictionary<string, object>();

        public Task SetAsync<TModel>(string key, TModel data, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where TModel : class, new()
        {
            _data[key] = data;

            return Task.CompletedTask;
        }

        public Task<TModel?> TryGetAsync<TModel>(string key, CancellationToken cancellationToken = default) where TModel : class, new()
        {
            if (_data.TryGetValue(key, out var data))
                return Task.FromResult<TModel?>((TModel)data);

            return Task.FromResult<TModel?>(null);
        }
    }
}
