using MattCanello.NewsFeed.RssReader.Domain.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Services;
using MattCanello.NewsFeed.RssReader.Infrastructure.Parsers;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using Microsoft.Extensions.Logging;
using Moq;
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

        [Fact]
        public void FromRSS_UsingTheVergeSample_ShouldReturnExceptedData()
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_verge));
            var feed = SyndicationFeed.Load(xml);

            var htmlContentParser = new HtmlContentParser(new Mock<ILogger<HtmlContentParser>>().Object);
            var contentParserEvaluator = new SingleContentParserEvaluator(htmlContentParser);
            var factory = new EntryFactory(EmptyNonStandardEnricherEvaluator.Instance, contentParserEvaluator);
            var entries = factory.FromRSS(feed);

            Assert.NotNull(entries);
            var singleEntry = Assert.Single(entries);

            Assert.Equal("Ford looks to future EV breakthroughs — and smaller cars — to staunch the bleeding", singleEntry.Title);
            Assert.Equal("<p id=\"3m9hP4\">Ford is the No. 2 seller of electric vehicles in the US. It’s very proud of that fact, but the amount of cash it had to burn to get there is enough to make you wonder whether it can keep that title.  </p><div class=\"c-float-left c-float-hang\"><aside id=\"vbM4Cx\"><q>The company reported its first quarter earnings last night, and woo boy, it’s rough</q></aside></div><p id=\"oihQFq\">The company reported its first quarter earnings last night, and woo boy, it’s rough. Ford said it lost $1.3 billion on the sale of 10,000 electric vehicles in the first three months of the year — a staggering figure that amounts to $130,000 lost for every EV sold. </p><p id=\"NQVI22\">Ford’s Model e division, which oversees some of the company’s EV sales as well as software, reported $100 million in revenue, an 84 percent drop from the same period last year. The number of...</p><p><a href=\"https://www.theverge.com/2024/4/25/24140033/ford-q1-2024-earnings-ev-hybrid-loss-sale\">Continue reading…</a></p>", singleEntry.Description);
            Assert.Equal("https://www.theverge.com/2024/4/25/24140033/ford-q1-2024-earnings-ev-hybrid-loss-sale", singleEntry.Id);
            Assert.Equal(DateTimeOffset.Parse("2024-04-25T10:39:57-04:00"), singleEntry.PublishDate);
            Assert.Equal("https://www.theverge.com/2024/4/25/24140033/ford-q1-2024-earnings-ev-hybrid-loss-sale", singleEntry.Url);

            Assert.NotNull(singleEntry.Authors);
            var singleAuthor = Assert.Single(singleEntry.Authors);
            Assert.Equal("Andrew J. Hawkins", singleAuthor.Name);
            Assert.Null(singleAuthor.Email);
            Assert.Null(singleAuthor.URL);

            Assert.NotNull(singleEntry.Thumbnails);
            var singleThumb = Assert.Single(singleEntry.Thumbnails);
            Assert.Equal("https://cdn.vox-cdn.com/thumbor/GRJXTXw-GsfXlxEv1guyo6zrI1Q=/80x0:935x570/1310x873/cdn.vox-cdn.com/uploads/chorus_image/image/73304507/2024_ford_f_150_stx_exterior_106_64ff7327a5c0f.0.jpg", singleThumb.Url);
            Assert.Equal("Ford logo", singleThumb.Credit);

            Assert.NotNull(singleEntry.Categories);
            Assert.Empty(singleEntry.Categories);
        }
    }
}
