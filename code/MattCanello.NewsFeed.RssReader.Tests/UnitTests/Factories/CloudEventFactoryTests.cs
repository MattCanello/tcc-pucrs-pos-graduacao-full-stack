using AutoFixture.Xunit2;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Infrastructure.Factories;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Factories
{
    public sealed class CloudEventFactoryTests
    {
        [Theory, AutoData]
        public void CreateNewEntryEvent_WhenDataIsValid_ShouldProduceExpectedResult(string feedId, Entry entry, Uri entryUrl, Uri entryId)
        {
            entry.Url = entryUrl.ToString();
            entry.Id = entryId.ToString();

            var factory = new CloudEventFactory(MockedDateTimeProvider.Any);

            var cloudEvent = factory.CreateNewEntryEvent(feedId, entry);

            Assert.Equal("1.0", cloudEvent.SpecVersion.VersionId);
            Assert.Equal(entry, cloudEvent.Data);
            Assert.Equal($"/rss-reader/{feedId}", cloudEvent.Source!.ToString());
            Assert.Equal(entry.Id, cloudEvent.Subject);
            Assert.Equal(entry.Id, cloudEvent.Id);
            Assert.Equal(entry.PublishDate, cloudEvent.Time);
            Assert.Equal("mattcanello.newsfeed.newentry", cloudEvent.Type);
            Assert.Equal(feedId, cloudEvent.GetPartitionKey());
        }

        [Theory, AutoData]
        public void CreateChannelUpdatedEvent_WhenDataIsValid_ShouldProduceExpectedResult(string feedId, Channel channel, Uri channelUrl, DateTimeOffset utcNow)
        {
            channel.Url = channelUrl.ToString();

            var factory = new CloudEventFactory(new MockedDateTimeProvider(utcNow));

            var cloudEvent = factory.CreateChannelUpdatedEvent(feedId, channel);

            Assert.Equal("1.0", cloudEvent.SpecVersion.VersionId);
            Assert.Equal(channel, cloudEvent.Data);
            Assert.Equal($"/rss-reader/{feedId}", cloudEvent.Source!.ToString());
            Assert.Equal(feedId, cloudEvent.Subject);
            Assert.Equal(feedId, cloudEvent.Id);
            Assert.Equal(utcNow, cloudEvent.Time);
            Assert.Equal("mattcanello.newsfeed.channelupdated", cloudEvent.Type);
            Assert.Equal(feedId, cloudEvent.GetPartitionKey());
        }
    }
}
