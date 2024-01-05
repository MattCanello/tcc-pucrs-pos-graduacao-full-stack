using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Models
{
    public sealed class FeedTests
    {
        [Theory, AutoData]
        public void SetAsModified_WhenLastETagIsNull_AndETagIsProvided_ShouldSetLastETag(string newETag)
        {
            var feed = new Feed() { LastETag = null };
            feed.SetAsModified(etag: newETag);

            Assert.Equal(newETag, feed.LastETag);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastETagIsNotNull_AndETagIsProvided_ShouldReplaceLastETag(string previousETag, string newETag)
        {
            var feed = new Feed() { LastETag = previousETag };
            feed.SetAsModified(etag: newETag);

            Assert.Equal(newETag, feed.LastETag);
        }

        [Fact]
        public void SetAsModified_WhenLastETagIsNull_AndETagIsNull_ShouldKeepLastETagNull()
        {
            var feed = new Feed() { LastETag = null };
            feed.SetAsModified(etag: null);

            Assert.Null(feed.LastETag);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastETagIsNotNull_AndETagIsNull_ShouldNotReplaceLastETag(string previousETag)
        {
            var feed = new Feed() { LastETag = previousETag };
            feed.SetAsModified(etag: null);

            Assert.Equal(previousETag, feed.LastETag);
        }

        //

        [Theory, AutoData]
        public void SetAsModified_WhenLastModifiedDateIsNull_AndModifiedDateIsProvided_ShouldSetLastModifiedDate(DateTimeOffset newModifiedDate)
        {
            var feed = new Feed() { LastModifiedDate = null };
            feed.SetAsModified(modifiedDate: newModifiedDate);

            Assert.Equal(newModifiedDate, feed.LastModifiedDate);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastModifiedDateIsNotNull_AndModifiedDateIsProvided_ShouldReplaceLastModifiedDate(DateTimeOffset previousModifiedDate, DateTimeOffset newModifiedDate)
        {
            var feed = new Feed() { LastModifiedDate = previousModifiedDate };
            feed.SetAsModified(modifiedDate: newModifiedDate);

            Assert.Equal(newModifiedDate, feed.LastModifiedDate);
        }

        [Fact]
        public void SetAsModified_WhenLastModifiedDateIsNull_AndModifiedDateIsNull_ShouldKeepLastModifiedDateNull()
        {
            var feed = new Feed() { LastModifiedDate = null };
            feed.SetAsModified(modifiedDate: null);

            Assert.Null(feed.LastModifiedDate);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastModifiedDateIsNotNull_AndModifiedDateIsNull_ShouldNotReplaceLastModifiedDate(DateTimeOffset previousModifiedDate)
        {
            var feed = new Feed() { LastModifiedDate = previousModifiedDate };
            feed.SetAsModified(modifiedDate: null);

            Assert.Equal(previousModifiedDate, feed.LastModifiedDate);
        }
    }
}
