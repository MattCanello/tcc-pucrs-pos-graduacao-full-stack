using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Domain.Application;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Tests.Mocks;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Application
{
    public class UpdateChannelAppTests
    {
        [Fact]
        public async Task UpdateChannelAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new UpdateChannelApp(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.UpdateChannelAsync(null!));

            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenUnknownFeedId_ShouldThrowException(UpdateChannelCommand command)
        {
            var app = new UpdateChannelApp(
                new MockedChannelService(),
                new MockedFeedRepository());

            var exception = await Assert.ThrowsAsync<FeedNotFoundException>(() => app.UpdateChannelAsync(command));

            Assert.Equal(exception.FeedId, command.FeedId);
        }

        [Theory,AutoData]
        public async Task UpdateChannelAsync_GivenNewChannel_ShouldCreateChannel(Feed feed, UpdateChannelCommand command)
        {
            feed.FeedId = command.FeedId!;
            var app = new UpdateChannelApp(
                new MockedChannelService(),
                new MockedFeedRepository(new Dictionary<string, Feed>
                {
                    { feed.FeedId, feed }
                }));

            var channel = await app.UpdateChannelAsync(command);

            Assert.NotNull(channel);
            Assert.NotNull(command.Channel);
            Assert.Equal(command.Channel.Name, channel.Name);
            Assert.Equal(command.Channel.Language, channel.Language);
            Assert.Equal(command.Channel.ImageUrl, channel.ImageUrl);
            Assert.Equal(command.Channel.Url, channel.Url);
            Assert.Equal(command.Channel.Copyright, channel.Copyright);
        }
    }
}
