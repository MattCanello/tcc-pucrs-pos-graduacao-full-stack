using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Factories;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Factories
{
    public sealed class ReadRssRequestMessageFactoryTests
    {
        [Fact]
        public void FromFeed_WhenFeedIsNull_ShouldThrowArgumentNullException()
        {
            var factory = new ReadRssRequestMessageFactory();

            Assert.Throws<ArgumentNullException>(() => factory.FromFeed(null!));
        }

        [Theory, AutoData]
        public void FromFeed_WhenFeedIsValid_ShouldReturnExpectedValues(Uri uri, Feed feed)
        {
            var factory = new ReadRssRequestMessageFactory();
            feed.Url = uri.ToString();

            var request = factory.FromFeed(feed);
            Assert.Equal(feed.LastETag, request.ETag);
            Assert.Equal(feed.LastModifiedDate, request.LastModifiedDate);
            Assert.Equal(feed.Url, request.Uri?.ToString());
        }
    }
}
