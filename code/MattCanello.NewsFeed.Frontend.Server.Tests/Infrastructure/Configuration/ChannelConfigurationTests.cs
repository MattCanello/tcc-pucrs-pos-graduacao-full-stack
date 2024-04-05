using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Configuration
{
    public class ChannelConfigurationTests
    {
        [Fact]
        public void ChannelListBulkCount_GivenEmptyConfiguration_ShouldReturnDefaultValue()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 0)).Build();

            var config = new ChannelConfiguration(configuration);

            var frontPageNumberOfArticles = config.ChannelListBulkCount();

            Assert.Equal(10, frontPageNumberOfArticles);
        }

        [Theory, AutoData]
        public void ChannelListBulkCount_GivenValidConfiguration_ShouldReturnExpectedValue(int channelListBulkCount)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 1) { { "CHANNEL_LIST_BULK_COUNT", channelListBulkCount.ToString("F0") } }).Build();

            var config = new ChannelConfiguration(configuration);

            var resultChannelListBulkCount = config.ChannelListBulkCount();

            Assert.Equal(channelListBulkCount, resultChannelListBulkCount);
        }

        [Theory, AutoData]
        public void ChannelListBulkCount_GivenInvalidConfiguration_ShouldReturnExpectedValue(string channelListBulkCount)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 1) { { "CHANNEL_LIST_BULK_COUNT", channelListBulkCount } }).Build();

            var config = new ChannelConfiguration(configuration);

            var resultChannelListBulkCount = config.ChannelListBulkCount();

            Assert.Equal(10, resultChannelListBulkCount);
        }
    }
}
