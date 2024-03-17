using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Models;
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

        [Fact]
        public void UpdateFeed_WhenGivenNull_ShouldThrowException()
        {
            var response = new PublishRssEntriesResponse();

            var exception = Assert.Throws<ArgumentNullException>(() => response.UpdateFeed(null!));

            Assert.Equal("feed", exception.ParamName);
        }

        [Theory, AutoData]
        public void UpdateFeed_WhenFeedLastPublishedEntryDateIsNull_ShouldCopyMostRecentPublishDate(Feed feed, PublishRssEntriesResponse response)
        {
            feed.LastPublishedEntryDate = null;

            response.UpdateFeed(feed);

            Assert.Equal(response.MostRecentPublishDate, feed.LastPublishedEntryDate);
        }

        [Theory, AutoData]
        public void UpdateFeed_WhenResponseMostRecentPublishDateIsNull_ShouldPreserveFeedLastPublishedEntryDate(Feed feed, PublishRssEntriesResponse response)
        {
            var feedLastPublishedDate = feed.LastPublishedEntryDate;

            response = response with { MostRecentPublishDate = null };

            response.UpdateFeed(feed);

            Assert.Equal(feedLastPublishedDate, feed.LastPublishedEntryDate);
        }
    }
}
