using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    sealed class MockedCachingConfiguration : ICachingConfiguration
    {
        private readonly TimeSpan _channelExpiryTime;
        private readonly TimeSpan _feedExpiryTime;

        public MockedCachingConfiguration(TimeSpan channelExpiryTime, TimeSpan feedExpiryTime)
        {
            _channelExpiryTime = channelExpiryTime;
            _feedExpiryTime = feedExpiryTime;
        }

        public TimeSpan GetChannelExpiryTime() 
            => _channelExpiryTime;

        public TimeSpan GetFeedExpiryTime() 
            => _feedExpiryTime;
    }
}
