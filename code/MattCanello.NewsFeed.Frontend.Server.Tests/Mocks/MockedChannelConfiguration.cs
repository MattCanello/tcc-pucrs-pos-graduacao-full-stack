using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    sealed class MockedChannelConfiguration : IChannelConfiguration
    {
        private readonly int _channelListBulkCount;

        public MockedChannelConfiguration(int channelListBulkCount) 
            => _channelListBulkCount = channelListBulkCount;

        public int ChannelListBulkCount() 
            => _channelListBulkCount;
    }
}
