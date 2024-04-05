using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Domain.Factories;
using System.Net;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Domain.Factories
{
    public class ArticleDetailsFactoryTests
    {
        [Fact]
        public void FromDescription_GivenNullDescription_ShouldReturnNullDetails()
        {
            var factory = new ArticleDetailsFactory();

            var details = factory.FromDescription(null);

            Assert.Null(details);
        }

        [Fact]
        public void FromDescription_GivenEmptyLinesOnly_ShouldReturnNullDetails()
        {
            var description = Environment.NewLine + Environment.NewLine;

            var factory = new ArticleDetailsFactory();

            var details = factory.FromDescription(description);

            Assert.Null(details);
        }

        [Theory, AutoData]
        public void FromDescription_GivenManyLines_ShouldSplitLines(string line1, string line2)
        {
            var description = $"{line1}{Environment.NewLine}{line2}";

            var factory = new ArticleDetailsFactory();

            var details = factory.FromDescription(description);

            Assert.NotNull(details);
            Assert.Equal(line1, details.Summary);
            Assert.NotNull(details.Lines);

            var singleLine = Assert.Single(details.Lines);
            Assert.Equal(line2, singleLine);
        }

        [Theory, AutoData]
        public void FromDescription_GivenContinueReading_ShouldNotIncludeThatInLines(string line1, string line2)
        {
            var description = $"{line1}{Environment.NewLine}{line2}{Environment.NewLine}Continue reading";

            var factory = new ArticleDetailsFactory();

            var details = factory.FromDescription(description);

            Assert.NotNull(details);
            Assert.Equal(line1, details.Summary);
            Assert.NotNull(details.Lines);

            var singleLine = Assert.Single(details.Lines);
            Assert.Equal(line2, singleLine);
        }

        [Theory, AutoData]
        public void FromDescription_GivenHtmlEncoding_ShouldDecodeHtml(string line1)
        {
            var description = WebUtility.HtmlEncode(line1);

            var factory = new ArticleDetailsFactory();

            var details = factory.FromDescription(description);

            Assert.NotNull(details);
            Assert.Equal(line1, details.Summary);
            Assert.NotNull(details.Lines);
            Assert.Empty(details.Lines);
        }
    }
}
