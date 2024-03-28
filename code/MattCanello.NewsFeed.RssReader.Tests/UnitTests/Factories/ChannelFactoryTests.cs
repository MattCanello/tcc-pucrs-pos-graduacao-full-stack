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
            var channel = factory.FromRSS(feed);

            Assert.NotNull(channel);

            Assert.Equal("Guardian News and Media Limited or its affiliated companies. All rights reserved. 2024", channel.Copyright);
            Assert.Equal("Latest news, sport, business, comment, analysis and reviews from the Guardian, the world's leading liberal voice", channel.Description);
            Assert.Equal("en-gb", channel.Language);
            Assert.Equal("The Guardian", channel.Name);
            Assert.Equal("https://www.theguardian.com/uk", channel.Url);
            Assert.Equal("https://assets.guim.co.uk/images/guardian-logo-rss.c45beb1bafa34b347ac333af2e6fe23f.png", channel.ImageUrl);
        }
    }
}
