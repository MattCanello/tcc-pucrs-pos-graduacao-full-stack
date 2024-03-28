using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Exceptions
{
    public class FeedNotFoundExceptionTests
    {
        [Theory, AutoData]
        public void Constructor_GivenFeedId_ShouldPreserveFeedId(string feedId)
        {
            var ex = new FeedNotFoundException(feedId);

            Assert.Equal(feedId, ex.FeedId);
            Assert.Equal("The requested feed was not found", ex.Message);
        }
    }
}
