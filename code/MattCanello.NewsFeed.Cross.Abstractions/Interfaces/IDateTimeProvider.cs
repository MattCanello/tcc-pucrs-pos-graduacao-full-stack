namespace MattCanello.NewsFeed.Cross.Abstractions.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTimeOffset GetUtcNow();
    }
}
