﻿using MattCanello.NewsFeed.RssReader.Services;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Services
{
    public sealed class EntryServiceTests
    {
        [Fact]
        public async Task ProcessEntriesFromRSSAsync__UsingTheGuardianUkSample_ShouldPublishEvent()
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var feed = SyndicationFeed.Load(xml);

            var publisher = new InMemoryEntryPublisher();
            var service = new EntryService(new EntryReader(EmptyNonStandardEnricherEvaluator.Instance), publisher);

            await service.ProcessEntriesFromRSSAsync(feed);

            Assert.NotNull(publisher.PublishedEntries);
            var singleEntry = Assert.Single(publisher.PublishedEntries);

            Assert.NotNull(singleEntry);
        }
    }
}
