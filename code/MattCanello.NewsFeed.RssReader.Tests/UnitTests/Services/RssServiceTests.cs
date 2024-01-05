using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Exceptions;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;
using MattCanello.NewsFeed.RssReader.Services;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Services
{
    public sealed class RssServiceTests
    {
        private const string ETag = "W/\"hash963960047320543834b\"";
        private static readonly DateTimeOffset LastModifiedDate = DateTimeOffset.Parse("Mon, 01 Jan 2024 15:28:01 GMT");

        private readonly IRssClient _rssClient = new RssClient(new HttpClient(new MockedRssHandler(Resources.sample_rss_the_guardian_uk, ETag, LastModifiedDate)));
        
        [Theory, AutoData]
        public async Task ProcessFeedAsync_OnFirstRequest_ShouldUpdateETagAndModifiedDate(Uri url, string feedId)
        {
            var feed = new Feed(feedId, url.ToString());

            var service = new RssService(
                new InMemoryFeedRepository(feed),
                Util.Mapper,
                _rssClient,
                new ChannelService(new ChannelReader(), new InMemoryChannelPublisher()),
                new EntryService(new EntryReader(EmptyNonStandardEnricherEvaluator.Instance), new InMemoryEntryPublisher())
                );

            await service.ProcessFeedAsync(feedId);

            Assert.Equal(LastModifiedDate, feed.LastModifiedDate);
            Assert.Equal(ETag, feed.LastETag);
        }

        [Theory, AutoData]
        public async Task ProcessFeedAsync_WithUnknownFeedId_ShouldThrowFeedNotFoundException(string feedId)
        {
            var service = new RssService(
                new InMemoryFeedRepository(),
                Util.Mapper,
                _rssClient,
                new ChannelService(new ChannelReader(), new InMemoryChannelPublisher()),
                new EntryService(new EntryReader(EmptyNonStandardEnricherEvaluator.Instance), new InMemoryEntryPublisher())
                );

            await Assert.ThrowsAsync<FeedNotFoundException>(() => service.ProcessFeedAsync(feedId));
        }

        [Theory, AutoData]
        public async Task ProcessFeedAsync_WhenFeedIsAlreadyUpdated_ShouldNotProduceEvents(Uri url, string feedId)
        {
            var feed = new Feed(feedId, url.ToString(), ETag, LastModifiedDate);

            var entryPublisher = new InMemoryEntryPublisher();
            var channelPublisher = new InMemoryChannelPublisher();

            var service = new RssService(
                new InMemoryFeedRepository(feed),
                Util.Mapper,
                _rssClient,
                new ChannelService(new ChannelReader(), channelPublisher),
                new EntryService(new EntryReader(EmptyNonStandardEnricherEvaluator.Instance), entryPublisher)
                );

            await service.ProcessFeedAsync(feedId);

            Assert.Empty(entryPublisher.PublishedEntries);
            Assert.Empty(channelPublisher.PublishedChannels);
        }
    }
}
