using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Configuration
{
    public class CachingConfigurationTests
    {
        [Fact]
        public void GetFeedExpiryTime_GivenEmptyConfiguration_ShouldReturnDefaultValue()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 0)).Build();

            var config = new CachingConfiguration(configuration);

            var resultingFeedExpiryTime = config.GetFeedExpiryTime();

            Assert.Equal(TimeSpan.FromMinutes(10), resultingFeedExpiryTime);
        }

        [Theory, AutoData]
        public void GetFeedExpiryTime_GivenValidConfiguration_ShouldReturnExpectedValue(TimeSpan feedExpiryTime)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 1) { { "FEED_EXPIRY_TIME", feedExpiryTime.ToString() } }).Build();

            var config = new CachingConfiguration(configuration);

            var resultingFeedExpiryTime = config.GetFeedExpiryTime();

            Assert.Equal(feedExpiryTime, resultingFeedExpiryTime);
        }

        [Theory, AutoData]
        public void GetFeedExpiryTime_GivenInvalidConfiguration_ShouldReturnExpectedValue(string feedExpiryTime)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 1) { { "FEED_EXPIRY_TIME", feedExpiryTime } }).Build();

            var config = new CachingConfiguration(configuration);

            var resultingFeedExpiryTime = config.GetFeedExpiryTime();

            Assert.Equal(TimeSpan.FromMinutes(10), resultingFeedExpiryTime);
        }

        [Fact]
        public void GetChannelExpiryTime_GivenEmptyConfiguration_ShouldReturnDefaultValue()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 0)).Build();

            var config = new CachingConfiguration(configuration);

            var resultingChannelExpiryTime = config.GetChannelExpiryTime();

            Assert.Equal(TimeSpan.FromMinutes(10), resultingChannelExpiryTime);
        }

        [Theory, AutoData]
        public void GetChannelExpiryTime_GivenValidConfiguration_ShouldReturnExpectedValue(TimeSpan channelExpiryTime)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 1) { { "CHANNEL_EXPIRY_TIME", channelExpiryTime.ToString() } }).Build();

            var config = new CachingConfiguration(configuration);

            var resultingChannelExpiryTime = config.GetChannelExpiryTime();

            Assert.Equal(channelExpiryTime, resultingChannelExpiryTime);
        }

        [Theory, AutoData]
        public void GetChannelExpiryTime_GivenInvalidConfiguration_ShouldReturnExpectedValue(string channelExpiryTime)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 1) { { "CHANNEL_EXPIRY_TIME", channelExpiryTime } }).Build();

            var config = new CachingConfiguration(configuration);

            var resultingChannelExpiryTime = config.GetChannelExpiryTime();

            Assert.Equal(TimeSpan.FromMinutes(10), resultingChannelExpiryTime);
        }
    }
}
