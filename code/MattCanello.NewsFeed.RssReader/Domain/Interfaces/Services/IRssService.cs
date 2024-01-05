namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services
{
    public interface IRssService
    {
        Task ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default);
    }
}
