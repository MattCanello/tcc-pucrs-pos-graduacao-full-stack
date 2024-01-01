namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IRssService
    {
        Task ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default);
    }
}
