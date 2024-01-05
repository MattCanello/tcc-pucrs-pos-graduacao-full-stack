using AutoFixture.Xunit2;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Infrastructure.Factories;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Factories
{
    public sealed class CloudEventFactoryTests
    {
        [Theory, AutoData]
        public void CreateNewEntryFoundEvent_WhenDataIsValid_ShouldProduceExpectedResult(string feedId, Entry entry, Uri entryUrl, Uri entryId)
        {
            entry.Url = entryUrl.ToString();
            entry.Id = entryId.ToString();

            var factory = new CloudEventFactory();

            var cloudEvent = factory.CreateNewEntryFoundEvent(feedId, entry);

            Assert.Equal("1.0", cloudEvent.SpecVersion.VersionId);
            Assert.Equal(entry, cloudEvent.Data);
            Assert.Equal($"/rss-reader/{feedId}", cloudEvent.Source!.ToString());
            Assert.Equal(entry.Id, cloudEvent.Subject);
            Assert.Equal(entry.Id, cloudEvent.Id);
            Assert.Equal(entry.PublishDate, cloudEvent.Time);
            Assert.Equal("mattcanello.newsfeed.newentryfound", cloudEvent.Type);
            Assert.Equal(feedId, cloudEvent.GetPartitionKey());
        }

        [Theory, AutoData]
        public void CreateFeedConsumedEvent_WhenDataIsValid_ShouldProduceExpectedResult(string feedId, Channel channel, Uri channelUrl, DateTimeOffset consumedDate)
        {
            channel.Url = channelUrl.ToString();

            var factory = new CloudEventFactory();

            var cloudEvent = factory.CreateFeedConsumedEvent(feedId, consumedDate, channel);

            Assert.Equal("1.0", cloudEvent.SpecVersion.VersionId);
            Assert.Equal(channel, cloudEvent.Data);
            Assert.Equal($"/rss-reader/{feedId}", cloudEvent.Source!.ToString());
            Assert.Equal(feedId, cloudEvent.Subject);
            Assert.Equal(feedId, cloudEvent.Id);
            Assert.Equal(consumedDate, cloudEvent.Time);
            Assert.Equal("mattcanello.newsfeed.feedconsumed", cloudEvent.Type);
            Assert.Equal(feedId, cloudEvent.GetPartitionKey());
        }
    }
}
