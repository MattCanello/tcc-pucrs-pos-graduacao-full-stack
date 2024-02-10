using AutoFixture.Xunit2;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.CronApi.Infrastructure.Factories;
using MattCanello.NewsFeed.Cross.Abstractions.Tests.Mocks;

namespace MattCanello.NewsFeed.CronApi.Tests.UnitTests.Factories
{
    public class CloudEventFactoryTests
    {
        [Fact]
        public void CreateProcessRssEvent_GivenNullFeedId_ShouldThrowException()
        {
            var factory = new CloudEventFactory(new MockedDateTimeProvider());
            Assert.Throws<ArgumentNullException>(() => factory.CreateProcessRssEvent(null!));
        }

        [Theory, AutoData]
        public void CreateProcessRssEvent_GivenValidFeedId_ShouldProduceExcepectedEvent(string feedId)
        {
            var now = DateTimeOffset.UtcNow;
            var factory = new CloudEventFactory(new MockedDateTimeProvider(now));

            var cloudEvent = factory.CreateProcessRssEvent(feedId);

            Assert.NotNull(cloudEvent.Time);
            Assert.Equal(now, cloudEvent.Time.Value);
            
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
