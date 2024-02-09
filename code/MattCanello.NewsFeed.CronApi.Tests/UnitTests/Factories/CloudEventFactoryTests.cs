using AutoFixture.Xunit2;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.CronApi.Infrastructure.Factories;

namespace MattCanello.NewsFeed.CronApi.Tests.UnitTests.Factories
{
    public class CloudEventFactoryTests
    {
        [Fact]
        public void CreateProcessRssEvent_GivenNullFeedId_ShouldThrowException()
        {
            var factory = new CloudEventFactory();
            Assert.Throws<ArgumentNullException>(() => factory.CreateProcessRssEvent(null!));
        }

        [Theory, AutoData]
        public void CreateProcessRssEvent_GivenValidFeedId_ShouldProduceExcepectedEvent(string feedId)
        {
            var factory = new CloudEventFactory();

            var cloudEvent = factory.CreateProcessRssEvent(feedId);

            Assert.NotNull(cloudEvent.Time);
            Assert.Equal(DateTimeOffset.UtcNow.DateTime, cloudEvent.Time.Value.DateTime, TimeSpan.FromSeconds(1));
            
            Assert.NotNull(cloudEvent.Source);
            Assert.Equal($"/cron-api/{feedId}", cloudEvent.Source.ToString());

            Assert.Equal("1.0", cloudEvent.SpecVersion.VersionId);
            Assert.Equal("mattcanello.newsfeed.processrssfeed", cloudEvent.Type);

            Assert.Equal(feedId, cloudEvent.Id);
            Assert.Equal(feedId, cloudEvent.Subject);
            Assert.Equal(feedId, cloudEvent.GetPartitionKey());
        }
    }
}
