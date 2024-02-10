using AutoFixture.Xunit2;
using MattCanello.NewsFeed.CronApi.Domain.Applications;
using MattCanello.NewsFeed.CronApi.Domain.Models;
using MattCanello.NewsFeed.CronApi.Tests.Fakes;

namespace MattCanello.NewsFeed.CronApi.Tests.UnitTests.Applications
{
    public class CronPublishAppTests
    {
        [Fact]
        public async Task PublishSlotAsync_GivenNoFeedIds_ShouldReturnZeroAsync()
        {
            const byte slot = 0;
            var feedRepository = new FakeFeedRepository(new Dictionary<byte, IDictionary<string, Feed>>(capacity: 0));

            var app = new CronPublishApp(feedRepository, new FakeCronFeedEnqueuer());

            var result = await app.PublishSlotAsync(slot);
            Assert.Equal(0, result);
        }

        [Theory, AutoData]
        public async Task PublishSlotAsync_GivenOneFeedId_OnCorrectSlot_ShouldReturnOne(Feed feed)
        {
            const byte slot = 0;
            var feedRepository = new FakeFeedRepository(new Dictionary<byte, IDictionary<string, Feed>>(capacity: 1)
            {
                {
                    slot,
                    new Dictionary<string, Feed>() { { feed.FeedId, feed } }
                }
            });

            var app = new CronPublishApp(feedRepository, new FakeCronFeedEnqueuer());

            var result = await app.PublishSlotAsync(slot);
            Assert.Equal(1, result);
        }

        [Theory, AutoData]
        public async Task PublishSlotAsync_GivenOneFeedId_OnDifferentSlot_ShouldReturnZero(Feed feed)
        {
            const byte slotToPublish = 0;
            const byte feedSlotNumber = 1;
            var feedRepository = new FakeFeedRepository(new Dictionary<byte, IDictionary<string, Feed>>(capacity: 1)
            {
                {
                    feedSlotNumber,
                    new Dictionary<string, Feed>() { { feed.FeedId, feed } }
                }
            });

            var app = new CronPublishApp(feedRepository, new FakeCronFeedEnqueuer());

            var result = await app.PublishSlotAsync(slotToPublish);
            Assert.Equal(0, result);
        }


        [Theory, AutoData]
        public async Task PublishSlotAsync_GivenOneFeedId_OnCorrectSlot_ShouldUpdateLastExecutionDate(Feed feed)
        {
            feed.LastExecutionDate = null;

            const byte slot = 0;
            var feedRepository = new FakeFeedRepository(new Dictionary<byte, IDictionary<string, Feed>>(capacity: 1)
            {
                {
                    slot,
                    new Dictionary<string, Feed>() { { feed.FeedId, feed } }
                }
            });

            var app = new CronPublishApp(feedRepository, new FakeCronFeedEnqueuer());

            var now = DateTimeOffset.UtcNow;
            await app.PublishSlotAsync(slot);

            Assert.NotNull(feed.LastExecutionDate);
            Assert.Equal(now.DateTime, feed.LastExecutionDate.Value.DateTime, TimeSpan.FromSeconds(1));
        }
    }
}
