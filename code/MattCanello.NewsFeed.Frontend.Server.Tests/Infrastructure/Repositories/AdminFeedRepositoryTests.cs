using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Profiles;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Repositories;
using MattCanello.NewsFeed.Frontend.Server.Tests.Mocks;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Repositories
{
    public class AdminFeedRepositoryTests
    {
        [Fact]
        public async Task GetFeedAndChannelAsync_GivenNullFeedId_ShouldThrowException()
        {
            var repository = new AdminFeedRepository(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.GetFeedAndChannelAsync(null!));

            Assert.Equal("feedId", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task GetFeedAndChannelAsync_GivenExistingFeedId_ShouldReturnExpectedData(AdminFeedWithChannel adminFeedWithChannel)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<AdminProfile>()).CreateMapper();

            var repository = new AdminFeedRepository(new MockedAdminClient(feedId => adminFeedWithChannel), mapper);

            var (feed, channel) = await repository.GetFeedAndChannelAsync(adminFeedWithChannel.FeedId!);

            Assert.NotNull(feed);
            Assert.NotNull(channel);

            Assert.Equal(adminFeedWithChannel.Name, feed.Name);
            Assert.Equal(adminFeedWithChannel.FeedId, feed.FeedId);

            Assert.NotNull(adminFeedWithChannel.Channel);
            Assert.Equal(adminFeedWithChannel.Channel.Name, channel.Name);
            Assert.Equal(adminFeedWithChannel.Channel.ChannelId, channel.ChannelId);
        }
    }
}
