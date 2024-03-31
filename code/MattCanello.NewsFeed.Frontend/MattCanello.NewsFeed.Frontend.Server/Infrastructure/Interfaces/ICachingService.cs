namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces
{
    public interface ICachingService
    {
        Task<TModel?> TryGetAsync<TModel>(string key, CancellationToken cancellationToken = default) where TModel : class, new();

        Task SetAsync<TModel>(string key, TModel data, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where TModel : class, new();
    }
}
