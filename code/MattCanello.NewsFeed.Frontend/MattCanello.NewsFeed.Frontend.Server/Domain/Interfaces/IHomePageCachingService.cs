namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IHomePageCachingService
    {
        Task<DateTimeOffset?> GetLatestArticleDateAsync(CancellationToken cancellationToken = default);

        Task SetLastestArticleDateAsync(DateTimeOffset publishDate,  CancellationToken cancellationToken = default);
    }
}
