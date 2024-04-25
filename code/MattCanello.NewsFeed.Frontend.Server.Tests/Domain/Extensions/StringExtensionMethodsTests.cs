using MattCanello.NewsFeed.Frontend.Server.Domain.Extensions;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Domain.Extensions
{
    public class StringExtensionMethodsTests
    {
        [Fact]
        public void TrimEllipseLastLine_GivenNullContent_ShouldReturnNull()
        {
            var trimmed = StringExtensionMethods.TrimEllipseLastLine(null!);

            Assert.Null(trimmed);
        }

        [Fact]
        public void TrimEllipseLastLine_GivenEmptyContent_ShouldReturnStringEmpty()
        {
            var trimmed = StringExtensionMethods.TrimEllipseLastLine(string.Empty);

            Assert.NotNull(trimmed);
            Assert.Empty(trimmed);
        }

        [Fact]
        public void TrimEllipseLastLine_GivenRegularLine_ShouldReturnInputString()
        {
            var trimmed = StringExtensionMethods.TrimEllipseLastLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");

            Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", trimmed);
        }

        [Fact]
        public void TrimEllipseLastLine_GivenEllipseLine_ShouldReturnTrimmedString()
        {
            var trimmed = StringExtensionMethods.TrimEllipseLastLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Velit egestas dui id ornare arcu odio ut sem nulla...");

            Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", trimmed);
        }

        [Fact]
        public void TrimEllipseLastLine_GivenOnlyEllipseLine_ShouldReturnStringEmpty()
        {
            var trimmed = StringExtensionMethods.TrimEllipseLastLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua...");

            Assert.NotNull(trimmed);
            Assert.Empty(trimmed);
        }

        [Fact]
        public void TrimEllipseLastLine_GivenTwiceEllipseLine_ShouldReturnTrimmedString()
        {
            var trimmed = StringExtensionMethods.TrimEllipseLastLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Velit egestas dui id ornare arcu odio ut sem nulla... Et netus et malesuada fames. Nulla malesuada pellentesque elit eget gravida...");

            Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Velit egestas dui id ornare arcu odio ut sem nulla... Et netus et malesuada fames.", trimmed);
        }
    }
}
