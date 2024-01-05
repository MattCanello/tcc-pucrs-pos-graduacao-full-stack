using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Application;
using MattCanello.NewsFeed.RssReader.Domain.Exceptions;
using MattCanello.NewsFeed.RssReader.Domain.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Clients;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Services;
using MattCanello.NewsFeed.RssReader.Infrastructure.Clients;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Application
{
    public sealed class RssAppTests
    {
        private const string ETag = "W/\"hash963960047320543834b\"";
        private static readonly DateTimeOffset LastModifiedDate = DateTimeOffset.Parse("Mon, 01 Jan 2024 15:28:01 GMT");

        private readonly IRssClient _rssClient = new RssClient(new HttpClient(new MockedRssHandler(Resources.sample_rss_the_guardian_uk, ETag, LastModifiedDate)));

        [Theory, AutoData]
        public async Task ProcessFeedAsync_OnFirstRequest_ShouldUpdateETagAndModifiedDate(Uri url, string feedId)
        {
            var feed = new Feed(feedId, url.ToString());

            var service = new RssApp(
                new InMemoryFeedRepository(feed),
                Util.Mapper,
                _rssClient,
                new ChannelService(new ChannelFactory(), new InMemoryChannelPublisher()),
                new EntryService(new EntryFactory(EmptyNonStandardEnricherEvaluator.Instance), new InMemoryEntryPublisher())
                );

            await service.ProcessFeedAsync(feedId);

            Assert.Equal(LastModifiedDate, feed.LastModifiedDate);
            Assert.Equal(ETag, feed.LastETag);
        }

        [Theory, AutoData]
        public async Task ProcessFeedAsync_WithUnknownFeedId_ShouldThrowFeedNotFoundException(string feedId)
        {
            var service = new RssApp(
                new InMemoryFeedRepository(),
                Util.Mapper,
                _rssClient,
                new ChannelService(new ChannelFactory(), new InMemoryChannelPublisher()),
                new EntryService(new EntryFactory(EmptyNonStandardEnricherEvaluator.Instance), new InMemoryEntryPublisher())
                );

            await Assert.ThrowsAsync<FeedNotFoundException>(() => service.ProcessFeedAsync(feedId));
        }

        [Theory, AutoData]
        public async Task ProcessFeedAsync_WhenFeedIsAlreadyUpdated_ShouldNotProduceEvents(Uri url, string feedId)
        {
            var feed = new Feed(feedId, url.ToString(), ETag, LastModifiedDate);

            var entryPublisher = new InMemoryEntryPublisher();
            var channelPublisher = new InMemoryChannelPublisher();

            var service = new RssApp(
                new InMemoryFeedRepository(feed),
                Util.Mapper,
                _rssClient,
                new ChannelService(new ChannelFactory(), channelPublisher),
                new EntryService(new EntryFactory(EmptyNonStandardEnricherEvaluator.Instance), entryPublisher)
                );

            await service.ProcessFeedAsync(feedId);

            Assert.Empty(entryPublisher.PublishedEntries);
            Assert.Empty(channelPublisher.PublishedChannels);
        }
    }
}
