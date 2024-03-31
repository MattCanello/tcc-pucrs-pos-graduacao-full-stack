using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Configuration
{
    sealed class ChannelConfiguration : IChannelConfiguration
    {
        private readonly int _channelListBulkCount;

        public ChannelConfiguration(IConfiguration configuration)
        {
            if (!int.TryParse(configuration[EnvironmentVariables.CHANNEL_LIST_BULK_COUNT], out _channelListBulkCount))
                _channelListBulkCount = 10;
        }

        public int ChannelListBulkCount()
            => _channelListBulkCount;
    }
}
