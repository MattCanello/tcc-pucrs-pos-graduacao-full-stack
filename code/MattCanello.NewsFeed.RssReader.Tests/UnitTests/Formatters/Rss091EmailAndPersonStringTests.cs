using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Formatters;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Formatters
{
    public sealed class Rss091EmailAndPersonStringTests
    {
        [Fact]
        public void TryParse_WithEmailAndDisplayName_ShouldResultExpectedReturn()
        {
            var tryParseResult = Rss091EmailAndPersonString.TryParse("dave@userland.com (Dave Winer)", out var emailAndPeson);

            Assert.True(tryParseResult);

            Assert.NotNull(emailAndPeson);
            Assert.Equal("dave@userland.com", emailAndPeson.Email);
            Assert.Equal("Dave Winer", emailAndPeson.DisplayName);
        }

        [Fact]
        public void TryParse_WithEmailAndNoName_ShouldResultExpectedReturn()
        {
            var tryParseResult = Rss091EmailAndPersonString.TryParse("dave@userland.com", out var emailAndPeson);

            Assert.True(tryParseResult);

            Assert.NotNull(emailAndPeson);
            Assert.Equal("dave@userland.com", emailAndPeson.Email);
            Assert.Null(emailAndPeson.DisplayName);
        }

        [Theory, AutoData]
        public void TryParse_NoEmailJustName_ShouldResultExpectedReturn(string input)
        {
            var tryParseResult = Rss091EmailAndPersonString.TryParse(input, out var emailAndPeson);

            Assert.False(tryParseResult);

            Assert.Null(emailAndPeson);
        }

        [Theory, AutoData]
        public void ToString_EmailAndDisplayName_ShouldReturnExpectedFormat(Rss091EmailAndPersonString data)
        {
            string toString = data.ToString();

            Assert.Equal($"{data.Email} ({data.DisplayName})", toString);
        }

        [Theory, AutoData]
        public void ToString_OnlyEmail_ShouldReturnExpectedFormat(string email)
        {
            var data = new Rss091EmailAndPersonString(email);

            string toString = data.ToString();

            Assert.Equal(email, toString);
        }
    }
}
