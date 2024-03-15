using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Responses
{
    public class FeedResponseTests
    {
        [Fact]
        public void Constructor_GivenNoParam_ShouldCreateEmptyInstance()
        {
            var response = new FeedResponse();

            Assert.Equal(0, response.Total);
            Assert.Null(response.Results);
            Assert.True(response.IsEmpty);
        }

        [Theory, AutoData]
        public void Constructor_GivenSpecificParams_ShouldPreserveGivenData(IReadOnlyList<Document> results, long total)
        {
            var response = new FeedResponse(results, total);

            Assert.Equal(total, response.Total);
            Assert.Equal(results, response.Results);
            Assert.False(response.IsEmpty);
        }
    }
}
