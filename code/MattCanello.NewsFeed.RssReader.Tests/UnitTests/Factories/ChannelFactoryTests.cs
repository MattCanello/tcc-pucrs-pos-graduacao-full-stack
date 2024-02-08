using MattCanello.NewsFeed.RssReader.Domain.Factories;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Factories
{
    public sealed class ChannelFactoryTests
    {
        [Fact]
        public void FromRSS_UsingTheGuardianUkSample_ShouldReturnExpectedData()
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var feed = SyndicationFeed.Load(xml);

            var factory = new ChannelFactory();
            var publisher = factory.FromRSS(feed);

            Assert.NotNull(publisher);

            Assert.Equal("Guardian News and Media Limited or its affiliated companies. All rights reserved. 2024", publisher.Copyright);
            Assert.Equal("Latest news, sport, business, comment, analysis and reviews from the Guardian, the world's leading liberal voice", publisher.Description);
            Assert.Equal("en-gb", publisher.Language);
            Assert.Equal("The Guardian", publisher.Name);
            Assert.Equal("https://www.theguardian.com/uk", publisher.Url);

            Assert.NotNull(publisher.Image);
            Assert.Equal("https://assets.guim.co.uk/images/guardian-logo-rss.c45beb1bafa34b347ac333af2e6fe23f.png", publisher.Image.Url);
        }
    }
}
