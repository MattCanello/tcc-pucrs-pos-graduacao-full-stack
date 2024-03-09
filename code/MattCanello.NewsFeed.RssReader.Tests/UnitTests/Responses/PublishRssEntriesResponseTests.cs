using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Responses;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Responses
{
    public class PublishRssEntriesResponseTests
    {
        [Fact]
        public void Constructor_GivenInvalidPublishedCount_ShouldThrowException()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new PublishRssEntriesResponse(-1, null));

            Assert.Equal(-1, exception.ActualValue);
            Assert.Equal("publishedCount", exception.ParamName);
        }

        [Theory, AutoData]
        public void Constructor_GivenValidInput_ShouldPreserveData(int publishedCount, DateTimeOffset mostRecentPublishDate)
        {
            var response = new PublishRssEntriesResponse(publishedCount, mostRecentPublishDate);

            Assert.Equal(publishedCount, response.PublishedCount);
            Assert.Equal(mostRecentPublishDate, response.MostRecentPublishDate);
        }
    }
}
