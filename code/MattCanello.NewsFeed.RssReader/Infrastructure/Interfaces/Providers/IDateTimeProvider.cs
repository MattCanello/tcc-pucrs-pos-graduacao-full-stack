namespace MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Providers
{
    public interface IDateTimeProvider
    {
        DateTimeOffset GetUtcNow();
    }
}
