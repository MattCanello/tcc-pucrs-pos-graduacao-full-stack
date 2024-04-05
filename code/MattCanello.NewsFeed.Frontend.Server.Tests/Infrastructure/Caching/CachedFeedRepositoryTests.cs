using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Caching;
using MattCanello.NewsFeed.Frontend.Server.Tests.Mocks;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Caching
{
    public class CachedFeedRepositoryTests
    {
        [Fact]
        public async Task GetFeedAndChannelAsync_GivenNullFeedId_ShouldThrowExcpetion()
        {
            var repository = new CachedFeedRepository(null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.GetFeedAndChannelAsync(null!));

            Assert.Equal("feedId", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task GetFeedAndChannelAsync_GivenEmptyCache_ShouldCallInnerRepository(Feed feed, Channel channel, TimeSpan channelExpiry, TimeSpan feedExpiry)
        {
            var feedId = feed.FeedId!;
            var calledInnerRepository = false;

            var repository = new CachedFeedRepository(
                new MockedFeedRepository((feedId) => { calledInnerRepository = true; return (feed, channel); }),
                new MockedCachingService(),
                new MockedCachingConfiguration(channelExpiry, feedExpiry));

            var (resultingFeed, resultingChannel) = await repository.GetFeedAndChannelAsync(feedId);

            Assert.Equal(feed, resultingFeed);
            Assert.Equal(channel, resultingChannel);
            Assert.True(calledInnerRepository);
        }

        [Theory, AutoData]
        public async Task GetFeedAndChannelAsync_GivenFeedNotPresentInCache_ShouldCallInnerRepository(string otherFeedId, Feed feed, Channel channel, TimeSpan channelExpiry, TimeSpan feedExpiry)
        {
            var feedId = feed.FeedId!;
            var calledInnerRepository = false;

            var repository = new CachedFeedRepository(
                new MockedFeedRepository((feedId) => { calledInnerRepository = true; return (feed, channel); }),
                new MockedCachingService(new Dictionary<string, object>(capacity: 2) { { CachedFeedRepository.GetFeedCacheKey(otherFeedId), feed }, { CachedFeedRepository.GetChannelCacheKey(otherFeedId), channel }, }),
                new MockedCachingConfiguration(channelExpiry, feedExpiry));

            var (resultingFeed, resultingChannel) = await repository.GetFeedAndChannelAsync(feedId);

            Assert.Equal(feed, resultingFeed);
            Assert.Equal(channel, resultingChannel);
            Assert.True(calledInnerRepository);
        }


        [Theory, AutoData]
        public async Task GetFeedAndChannelAsync_GivenFeedPresentInCache_ShouldNotCallInnerRepository(Feed feed, Channel channel, TimeSpan channelExpiry, TimeSpan feedExpiry)
        {
            var feedId = feed.FeedId!;
            var calledInnerRepository = false;

            var repository = new CachedFeedRepository(
                new MockedFeedRepository((feedId) => { calledInnerRepository = true; return (feed, channel); }),
                new MockedCachingService(new Dictionary<string, object>(capacity: 2) { { CachedFeedRepository.GetFeedCacheKey(feedId), feed }, { CachedFeedRepository.GetChannelCacheKey(feedId), channel }, }),
                new MockedCachingConfiguration(channelExpiry, feedExpiry));

            var (resultingFeed, resultingChannel) = await repository.GetFeedAndChannelAsync(feedId);

            Assert.Equal(feed, resultingFeed);
            Assert.Equal(channel, resultingChannel);
            Assert.False(calledInnerRepository);
        }
    }
}
