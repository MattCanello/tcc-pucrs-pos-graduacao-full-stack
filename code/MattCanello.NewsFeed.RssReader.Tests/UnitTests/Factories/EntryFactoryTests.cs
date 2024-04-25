using MattCanello.NewsFeed.RssReader.Domain.Factories;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Factories
{
    public sealed class EntryFactoryTests
    {
        [Fact]
        public void FromRSS_UsingTheGuardianUkSample_ShouldReturnExceptedData()
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var feed = SyndicationFeed.Load(xml);

            var factory = new EntryFactory(EmptyNonStandardEnricherEvaluator.Instance, NoContentParserEvaluator.Instance);
            var entries = factory.FromRSS(feed);

            Assert.NotNull(entries);
            var singleEntry = Assert.Single(entries);

            Assert.Equal("Japan orders people to evacuate after 7.6-magnitude quake hits west coast", singleEntry.Title);
            Assert.Equal("<p>Major tsunami warnings downgraded but residents in coastal areas told not to return to homes</p><p>• <a href=\"https://www.theguardian.com/world/live/2024/jan/01/japan-tsunami-earthquake-warning-latest-updates-live\">Japan quake: latest updates</a></p><p>A powerful earthquake has struck central Japan’s western coastline, triggering waves over a metre high and prompting tsunami alerts and warnings for people to evacuate.</p><p>The quake, which is estimated to have been magnitude 7.6, struck the Noto peninsula in Ishikawa prefecture on the main central island of Honshu at about 4.10pm local time (07:10 GMT). It knocked out power to tens of thousands of homes and disrupted flights and rail services.</p> <a href=\"https://www.theguardian.com/world/2024/jan/01/japan-issues-major-tsunami-warning-for-coastal-prefecture-after-76-magnitude-earthquake\">Continue reading...</a>", singleEntry.Description);
            Assert.Equal("https://www.theguardian.com/world/2024/jan/01/japan-issues-major-tsunami-warning-for-coastal-prefecture-after-76-magnitude-earthquake", singleEntry.Id);
            Assert.Equal(DateTimeOffset.Parse("Mon, 01 Jan 2024 15:28:01 GMT"), singleEntry.PublishDate);
            Assert.Equal("https://www.theguardian.com/world/2024/jan/01/japan-issues-major-tsunami-warning-for-coastal-prefecture-after-76-magnitude-earthquake", singleEntry.Url);

            Assert.NotNull(singleEntry.Authors);
            Assert.Empty(singleEntry.Authors);

            Assert.NotNull(singleEntry.Thumbnails);
            Assert.Empty(singleEntry.Thumbnails);

            Assert.NotNull(singleEntry.Categories);

            var singleCategory = Assert.Single(singleEntry.Categories);
            Assert.Null(singleCategory.Label);
            Assert.Equal("https://www.theguardian.com/world/japan", singleCategory.Schema);
            Assert.Equal("Japan", singleCategory.CategoryName);
        }
    }
}
