using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Services;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Services
{
    public sealed class ChannelServiceTests
    {
        [Theory, AutoData]
        public async Task ProcessChannelUpdateFromRssAsync_UsingTheGuardianUkSample_ShouldPublishEvent(string feedId)
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var feed = SyndicationFeed.Load(xml);

            var publisher = new InMemoryChannelPublisher();
            var service = new ChannelService(new ChannelFactory(), publisher);

            await service.ProcessChannelUpdateFromRssAsync(feedId, feed);

            Assert.NotNull(publisher.PublishedChannels);
            var singleChannel = Assert.Single(publisher.PublishedChannels);

            Assert.NotNull(singleChannel);

            var singleFeedId = Assert.Single(publisher.PublishedFeedIds);
            Assert.Same(feedId, singleFeedId);
        }
    }
}
