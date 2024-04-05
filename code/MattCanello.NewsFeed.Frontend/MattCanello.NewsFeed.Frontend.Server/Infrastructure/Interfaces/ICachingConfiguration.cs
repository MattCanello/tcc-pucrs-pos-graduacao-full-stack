namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces
{
    public interface ICachingConfiguration
    {
        TimeSpan GetFeedExpiryTime();
        TimeSpan GetChannelExpiryTime();
    }
}
