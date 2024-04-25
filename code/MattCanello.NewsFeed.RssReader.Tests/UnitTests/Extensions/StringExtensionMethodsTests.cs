using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Extensions;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Extensions
{
    public class StringExtensionMethodsTests
    {
        [Fact]
        public void ToNullWhenEmpty_WhenGivenNull_ShouldReturnNull()
        {
            var str = StringExtensionMethods.ToNullWhenEmpty(null);

            Assert.Null(str);
        }

        [Fact]
        public void ToNullWhenEmpty_WhenGivenStringEmpty_ShouldReturnNull()
        {
            var str = StringExtensionMethods.ToNullWhenEmpty(string.Empty);

            Assert.Null(str);
        }

        [Theory, AutoData]
        public void ToNullWhenEmpty_WhenGivenNotStringEmpty_ShouldReturnInputString(string input)
        {
            var str = StringExtensionMethods.ToNullWhenEmpty(input);

            Assert.Equal(input, str);
        }
    }
}
