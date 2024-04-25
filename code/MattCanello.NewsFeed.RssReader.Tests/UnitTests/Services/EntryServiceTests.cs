using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Evaluators;
using MattCanello.NewsFeed.RssReader.Domain.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Services;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Services
{
    public sealed class EntryServiceTests
    {
        [Theory, AutoData]
        public async Task ProcessEntriesFromRSSAsync_UsingTheGuardianUkSample_ShouldPublishEvent(Feed feed)
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var rss = SyndicationFeed.Load(xml);

            var publisher = new InMemoryEntryPublisher();
            var service = new EntryService(new EntryFactory(EmptyNonStandardEnricherEvaluator.Instance, NoContentParserEvaluator.Instance), publisher, new NoEntryPublishPolicy(), new MostRecentPublishDateEvaluator() );

            var publishedEntriesResponse = await service.ProcessEntriesFromRSSAsync(feed, rss);
            Assert.NotNull(publishedEntriesResponse);

            var publishedEntriesCount = publishedEntriesResponse.PublishedCount;

            Assert.NotNull(publisher.PublishedEntries);
            var singleEntry = Assert.Single(publisher.PublishedEntries);

            Assert.NotNull(singleEntry);
            Assert.Equal(1, publishedEntriesCount);
        }

        [Theory, AutoData]
        public async Task ProcessEntriesFromRSSAsync_GivenAPolicyThatPreventsPublishing_ShouldNotPublishAnyEvent(Feed feed)
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var rss = SyndicationFeed.Load(xml);

            var publisher = new InMemoryEntryPublisher();
            var service = new EntryService(new EntryFactory(EmptyNonStandardEnricherEvaluator.Instance, NoContentParserEvaluator.Instance), publisher, new PublishEntryMockedPolicy(false), new MostRecentPublishDateEvaluator());

            var publishedEntriesResponse = await service.ProcessEntriesFromRSSAsync(feed, rss);
            Assert.NotNull(publishedEntriesResponse);

            var publishedEntriesCount = publishedEntriesResponse.PublishedCount;

            Assert.NotNull(publisher.PublishedEntries);
            Assert.Empty(publisher.PublishedEntries);

            Assert.Equal(0, publishedEntriesCount);
        }
    }
}
