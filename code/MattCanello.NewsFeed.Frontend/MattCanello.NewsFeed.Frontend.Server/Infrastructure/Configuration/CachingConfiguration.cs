using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Configuration
{
    public sealed class CachingConfiguration : ICachingConfiguration
    {
        private readonly TimeSpan _feedExpiryTime;
        private readonly TimeSpan _channelExpiryTime;

        public CachingConfiguration(IConfiguration configuration)
        {
            if (!TimeSpan.TryParse(configuration[EnvironmentVariables.FEED_EXPIRY_TIME], out _feedExpiryTime))
                _feedExpiryTime = TimeSpan.FromMinutes(10);

            if (!TimeSpan.TryParse(configuration[EnvironmentVariables.CHANNEL_EXPIRY_TIME], out _channelExpiryTime))
                _channelExpiryTime = TimeSpan.FromMinutes(10);
        }

        public TimeSpan GetFeedExpiryTime()
            => _feedExpiryTime;

        public TimeSpan GetChannelExpiryTime()
            => _channelExpiryTime;
    }
}
