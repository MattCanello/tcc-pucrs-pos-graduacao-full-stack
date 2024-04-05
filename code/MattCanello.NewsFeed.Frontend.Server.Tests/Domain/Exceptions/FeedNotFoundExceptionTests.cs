using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Domain.Exceptions;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Domain.Exceptions
{
    public class FeedNotFoundExceptionTests
    {
        [Fact]
        public void Constructor_GivenNoParam_ShouldUseDefaultMessage()
        {
            var exception = new FeedNotFoundException();

            Assert.Equal("The requested feed was not found", exception.Message);
        }

        [Theory, AutoData]
        public void Constructor_GivenFeedId_ShouldPreserveFeedId(string feedId)
        {
            var exception = new FeedNotFoundException(feedId);

            Assert.Equal("The requested feed was not found", exception.Message);
            Assert.Equal(feedId, exception.FeedId);
        }

        [Theory, AutoData]
        public void Constructor_GivenFeedIdAndMessage_ShouldPreserveParams(string feedId, string message)
        {
            var exception = new FeedNotFoundException(feedId, message);

            Assert.Equal(message, exception.Message);
            Assert.Equal(feedId, exception.FeedId);
        }
    }
}
