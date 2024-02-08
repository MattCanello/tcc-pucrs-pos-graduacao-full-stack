using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Factories;
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
        public async Task ProcessEntriesFromRSSAsync_UsingTheGuardianUkSample_ShouldPublishEvent(string feedId)
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var feed = SyndicationFeed.Load(xml);

            var publisher = new InMemoryEntryPublisher();
            var service = new EntryService(new EntryFactory(EmptyNonStandardEnricherEvaluator.Instance), publisher);

            await service.ProcessEntriesFromRSSAsync(feedId, feed);

            Assert.NotNull(publisher.PublishedEntries);
            var singleEntry = Assert.Single(publisher.PublishedEntries);

            Assert.NotNull(singleEntry);
        }
    }
}
