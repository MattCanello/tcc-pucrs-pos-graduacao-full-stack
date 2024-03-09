using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Policies;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Policies
{
    public class ForwardOnlyPublishEntryPolicyTests
    {
        [Fact]
        public void ShouldPublish_GivenNullFeed_ShouldThrowException()
        {
            var policy = new ForwardOnlyPublishEntryPolicy();

            var exception = Assert.Throws<ArgumentNullException>(() => policy.ShouldPublish(null!, null!));

            Assert.Equal("feed", exception.ParamName);
        }

        [Theory, AutoData]
        public void ShouldPublish_GivenNullEntry_ShouldReturnFalse(Feed feed)
        {
            var policy = new ForwardOnlyPublishEntryPolicy();

            var shouldPublish = policy.ShouldPublish(feed, null!);

            Assert.False(shouldPublish);
        }

        [Theory, AutoData]
        public void ShouldPublish_GivenFeedWithoutLastPublishedDate_ShouldReturnTrue(Feed feed, Entry entry)
        {
            feed.LastPublishedEntryDate = null;
            var policy = new ForwardOnlyPublishEntryPolicy();

            var shouldPublish = policy.ShouldPublish(feed, entry);

            Assert.True(shouldPublish);
        }

        [Theory, AutoData]
        public void ShouldPublish_GivenFeedLastPublishedEntryDateGreaterThanEntryPublishDate_ShouldReturnFalse(Feed feed, Entry entry)
        {
            feed.LastPublishedEntryDate = new DateTimeOffset(2024, 03, 08, 21, 01, 00, TimeSpan.Zero);
            entry.PublishDate = new DateTimeOffset(2024, 03, 01, 21, 01, 00, TimeSpan.Zero);

            var policy = new ForwardOnlyPublishEntryPolicy();

            var shouldPublish = policy.ShouldPublish(feed, entry);

            Assert.False(shouldPublish);
        }

        [Theory, AutoData]
        public void ShouldPublish_GivenEntryPublishDateGreaterThanFeedLastPublishedEntryDateGreater_ShouldReturnTrue(Feed feed, Entry entry)
        {
            feed.LastPublishedEntryDate = new DateTimeOffset(2024, 03, 01, 21, 01, 00, TimeSpan.Zero);
            entry.PublishDate = new DateTimeOffset(2024, 03, 08, 21, 01, 00, TimeSpan.Zero);

            var policy = new ForwardOnlyPublishEntryPolicy();

            var shouldPublish = policy.ShouldPublish(feed, entry);

            Assert.True(shouldPublish);
        }

        [Theory, AutoData]
        public void ShouldPublish_GivenSameEntryPublishDateAndFeedLastPublishedEntryDateGreater_ShouldReturnTrue(Feed feed, Entry entry)
        {
            feed.LastPublishedEntryDate = entry.PublishDate = new DateTimeOffset(2024, 03, 08, 21, 01, 00, TimeSpan.Zero);

            var policy = new ForwardOnlyPublishEntryPolicy();

            var shouldPublish = policy.ShouldPublish(feed, entry);

            Assert.True(shouldPublish);
        }
    }
}
