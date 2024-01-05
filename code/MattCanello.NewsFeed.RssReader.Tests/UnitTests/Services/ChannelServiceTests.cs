using MattCanello.NewsFeed.RssReader.Services;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Services
{
    public sealed class ChannelServiceTests
    {
        [Fact]
        public async Task ProcessChannelUpdateFromRssAsync_UsingTheGuardianUkSample_ShouldPublishEvent()
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var feed = SyndicationFeed.Load(xml);

            var publisher = new InMemoryChannelPublisher();
            var service = new ChannelService(new ChannelReader(), publisher);

            await service.ProcessChannelUpdateFromRssAsync(feed);

            Assert.NotNull(publisher.PublishedChannels);
            var singleChannel = Assert.Single(publisher.PublishedChannels);

            Assert.NotNull(singleChannel);
        }
    }
}
