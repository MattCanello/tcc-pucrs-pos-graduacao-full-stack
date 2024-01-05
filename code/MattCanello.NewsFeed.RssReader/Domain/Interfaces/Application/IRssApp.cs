namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application
{
    public interface IRssApp
    {
        Task ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default);
    }
}
