using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.HTML;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.HTML
{
    public class HtmlHelpersTests
    {
        [Fact]
        public void StripHtmlTags_GivenNullHtml_ShouldReturnInputString()
        {
            string? inputHtml = null;

            var resultingString = inputHtml!.StripHtmlTags();

            Assert.Equal(inputHtml, resultingString);
        }

        [Fact]
        public void StripHtmlTags_GivenEmptyHtml_ShouldReturnInputString()
        {
            var inputHtml = string.Empty;

            var resultingString = inputHtml.StripHtmlTags();

            Assert.Equal(inputHtml, resultingString);
        }

        [Theory, AutoData]
        public void StripHtmlTags_GivenPTag_ShouldReplaceForEnvironmentNewLine(string part1, string part2)
        {
            var inputHtml = $"<p>{part1}</p><p>{part2}</p>";

            var resultingString = inputHtml.StripHtmlTags();

            Assert.Equal($"{part1}{Environment.NewLine}{Environment.NewLine}{part2}", resultingString);
        }

        [Theory, AutoData]
        public void StripHtmlTags_GivenBreakLineTag_ShouldReplaceForEnvironmentNewLine(string part1, string part2)
        {
            var inputHtml = $"{part1}<br/>{part2}";

            var resultingString = inputHtml.StripHtmlTags();

            Assert.Equal($"{part1}{Environment.NewLine}{part2}", resultingString);
        }

        [Theory, AutoData]
        public void StripHtmlTags_GivenListTags_ShouldReplaceForEnvironmentNewLine(string part1, string part2)
        {
            var inputHtml = $"<ul><li>{part1}</li><li>{part2}</li></ol>";

            var resultingString = inputHtml.StripHtmlTags();

            Assert.Equal($"{part1}{Environment.NewLine}{Environment.NewLine}{part2}", resultingString);
        }

        [Theory, AutoData]
        public void StripHtmlTags_GivenAnchorTags_ShouldRemoveLinks(Uri href, string title, string content)
        {
            var inputHtml = $"<a href=\"{href}\" title=\"{title}\">{content}</a>";

            var resultingString = inputHtml.StripHtmlTags();

            Assert.Equal(content, resultingString);
        }

        [Theory, AutoData]
        public void StripHtmlTags_GivenTagsToSupress_ShouldRemoveFromString(string content)
        {
            var inputHtml = $"<img /><span>{content}</span>";

            var resultingString = inputHtml.StripHtmlTags();

            Assert.Equal(content, resultingString);
        }
    }
}
