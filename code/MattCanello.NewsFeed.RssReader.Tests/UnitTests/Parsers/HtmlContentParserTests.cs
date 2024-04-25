using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Infrastructure.Parsers;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using Microsoft.Extensions.Logging;
using Moq;
using System.ServiceModel.Syndication;
using System.Xml;
using static MattCanello.NewsFeed.RssReader.Infrastructure.Parsers.HtmlContentParser;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Parsers
{
    public sealed class HtmlContentParserTests
    {
        [Fact]
        public void TryParse_GivenNullContent_ShouldThrowException()
        {
            var parser = new HtmlContentParser(new Mock<ILogger<HtmlContentParser>>().Object);

            var ex = Assert.Throws<ArgumentNullException>(() => parser.TryParse(null!));

            Assert.NotNull(ex);
            Assert.Equal("content", ex.ParamName);
        }

        [Theory, AutoData]
        public void TryParse_GiveNotHtmlContent_ShouldReturnNull(string contentText)
        {
            var parser = new HtmlContentParser(new Mock<ILogger<HtmlContentParser>>().Object);
            var content = new TextSyndicationContent(contentText, TextSyndicationContentKind.Plaintext);

            var parsedContent = parser.TryParse(content);

            Assert.Null(parsedContent);
        }

        [Fact]
        public void TryParse_GiveNotTextContent_ShouldReturnNull()
        {
            var parser = new HtmlContentParser(new Mock<ILogger<HtmlContentParser>>().Object);
            var content = new XmlSyndicationContent(XmlReader.Create(new StringReader("<root></root>")));

            var parsedContent = parser.TryParse(content);

            Assert.Null(parsedContent);
        }

        [Fact]
        public void TryParse_GiveEmptyHtmlContent_ShouldReturnNull()
        {
            var parser = new HtmlContentParser(new Mock<ILogger<HtmlContentParser>>().Object);
            var content = new TextSyndicationContent("", TextSyndicationContentKind.Html);

            var parsedContent = parser.TryParse(content);

            Assert.Null(parsedContent);
        }

        [Theory, AutoData]
        public void ToThumbnail_GivenValidData_ShouldResultExpectedObject(HtmlFigure htmlFigure)
        {
            var thumb = htmlFigure.ToThumbnail();

            Assert.NotNull(thumb);
            Assert.Equal(htmlFigure.Image!.Source, thumb.Url);
            Assert.Equal(htmlFigure.Image!.AltText, thumb.Credit);
        }

        [Theory, AutoData]
        public void ToThumbnail_GivenNullImageSource_ShouldReturnNull(HtmlFigure htmlFigure)
        {
            htmlFigure.Image!.Source = null;
            var thumb = htmlFigure.ToThumbnail();

            Assert.Null(thumb);
        }

        [Theory, AutoData]
        public void ToThumbnail_GivenNullImage_ShouldReturnNull(HtmlFigure htmlFigure)
        {
            htmlFigure.Image = null;
            var thumb = htmlFigure.ToThumbnail();

            Assert.Null(thumb);
        }

        [Theory, AutoData]
        public void ToThumbnail_GivenNullImageAltText_ShouldUseCaption(HtmlFigure htmlFigure)
        {
            htmlFigure.Image!.AltText = null;
            var thumb = htmlFigure.ToThumbnail();

            Assert.NotNull(thumb);
            Assert.Equal(htmlFigure.Image!.Source, thumb.Url);
            Assert.Equal(htmlFigure.Caption, thumb.Credit);
        }

        [Fact]
        public void TryParse_GivenValidEntry_ShouldParseData()
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_verge));
            var feed = SyndicationFeed.Load(xml);

            var parser = new HtmlContentParser(new Mock<ILogger<HtmlContentParser>>().Object);

            var content = parser.TryParse(feed.Items.First().Content);

            Assert.NotNull(content);
            Assert.Equal("<p id=\"3m9hP4\">Ford is the No. 2 seller of electric vehicles in the US. It’s very proud of that fact, but the amount of cash it had to burn to get there is enough to make you wonder whether it can keep that title.  </p><div class=\"c-float-left c-float-hang\"><aside id=\"vbM4Cx\"><q>The company reported its first quarter earnings last night, and woo boy, it’s rough</q></aside></div><p id=\"oihQFq\">The company reported its first quarter earnings last night, and woo boy, it’s rough. Ford said it lost $1.3 billion on the sale of 10,000 electric vehicles in the first three months of the year — a staggering figure that amounts to $130,000 lost for every EV sold. </p><p id=\"NQVI22\">Ford’s Model e division, which oversees some of the company’s EV sales as well as software, reported $100 million in revenue, an 84 percent drop from the same period last year. The number of...</p><p><a href=\"https://www.theverge.com/2024/4/25/24140033/ford-q1-2024-earnings-ev-hybrid-loss-sale\">Continue reading…</a></p>", content.Description);

            Assert.NotNull(content.Thumbnail);
            Assert.Equal("https://cdn.vox-cdn.com/thumbor/GRJXTXw-GsfXlxEv1guyo6zrI1Q=/80x0:935x570/1310x873/cdn.vox-cdn.com/uploads/chorus_image/image/73304507/2024_ford_f_150_stx_exterior_106_64ff7327a5c0f.0.jpg", content.Thumbnail.Url);
            Assert.Equal("Ford logo", content.Thumbnail.Credit);
        }
    }
}
