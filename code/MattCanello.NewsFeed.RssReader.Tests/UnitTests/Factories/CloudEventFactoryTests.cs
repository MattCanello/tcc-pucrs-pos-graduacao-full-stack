using AutoFixture.Xunit2;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.RssReader.Factories;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Factories
{
    public sealed class CloudEventFactoryTests
    {
        [Theory, AutoData]
        public void CreateNewEntryEvent_WhenDataIsValid_ShouldProduceExpectedResult(string feedId, Entry entry, Uri entryUrl, Uri entryId)
        {
            entry.Url = entryUrl.ToString();
            entry.Id = entryId.ToString();

            var factory = new CloudEventFactory();

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
    }
}
