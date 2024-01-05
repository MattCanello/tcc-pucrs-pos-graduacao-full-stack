using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Models
{
    public sealed class FeedTests
    {
        [Theory, AutoData]
        public void SetAsModified_WhenLastETagIsNull_AndETagIsProvided_ShouldSetLastETag(Feed feed, string newETag)
        {
            feed.LastETag = null;
            feed.SetAsModified(etag: newETag);

            Assert.Equal(newETag, feed.LastETag);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastETagIsNotNull_AndETagIsProvided_ShouldReplaceLastETag(Feed feed, string previousETag, string newETag)
        {
            feed.LastETag = previousETag;
            feed.SetAsModified(etag: newETag);

            Assert.Equal(newETag, feed.LastETag);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastETagIsNull_AndETagIsNull_ShouldKeepLastETagNull(Feed feed)
        {
            feed.LastETag = null;
            feed.SetAsModified(etag: null);

            Assert.Null(feed.LastETag);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastETagIsNotNull_AndETagIsNull_ShouldNotReplaceLastETag(Feed feed, string previousETag)
        {
            feed.LastETag = previousETag;
            feed.SetAsModified(etag: null);

            Assert.Equal(previousETag, feed.LastETag);
        }

        //

        [Theory, AutoData]
        public void SetAsModified_WhenLastModifiedDateIsNull_AndModifiedDateIsProvided_ShouldSetLastModifiedDate(Feed feed, DateTimeOffset newModifiedDate)
        {
            feed.LastModifiedDate = null;
            feed.SetAsModified(modifiedDate: newModifiedDate);

            Assert.Equal(newModifiedDate, feed.LastModifiedDate);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastModifiedDateIsNotNull_AndModifiedDateIsProvided_ShouldReplaceLastModifiedDate(Feed feed, DateTimeOffset previousModifiedDate, DateTimeOffset newModifiedDate)
        {
            feed.LastModifiedDate = previousModifiedDate;
            feed.SetAsModified(modifiedDate: newModifiedDate);

            Assert.Equal(newModifiedDate, feed.LastModifiedDate);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastModifiedDateIsNull_AndModifiedDateIsNull_ShouldKeepLastModifiedDateNull(Feed feed)
        {
            feed.LastModifiedDate = null;
            feed.SetAsModified(modifiedDate: null);

            Assert.Null(feed.LastModifiedDate);
        }

        [Theory, AutoData]
        public void SetAsModified_WhenLastModifiedDateIsNotNull_AndModifiedDateIsNull_ShouldNotReplaceLastModifiedDate(Feed feed, DateTimeOffset previousModifiedDate)
        {
            feed.LastModifiedDate = previousModifiedDate;
            feed.SetAsModified(modifiedDate: null);

            Assert.Equal(previousModifiedDate, feed.LastModifiedDate);
        }
    }
}
