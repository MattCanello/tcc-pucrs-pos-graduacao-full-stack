namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTimeOffset GetUtcNow();
    }
}
