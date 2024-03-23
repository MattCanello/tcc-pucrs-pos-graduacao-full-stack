using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Application;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;
using MattCanello.NewsFeed.AdminApi.Tests.Mocks;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Application
{
    public class CreateFeedAppTests
    {
        [Fact]
        public async Task CreateFeedAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new CreateFeedApp(null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.CreateFeedAsync(null!));

            Assert.NotNull(exception);
            Assert.Equal("createFeedCommand", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenNewFeed_ShouldCreateFeed(CreateFeedCommand command)
        {
            var app = new CreateFeedApp(
                new MockedFeedRepository(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper(),
                new MockedChannelService());

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.Equal(command.FeedId, feed.FeedId);
            Assert.Equal(command.Url, feed.Url);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenNewChannel_ShouldCreateChannel(CreateFeedCommand command)
        {
            var channelRepository = new MockedChannelRepository();

            var app = new CreateFeedApp(
                new MockedFeedRepository(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper(),
                new MockedChannelService(channelRepository));

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.NotNull(feed.Channel);
            Assert.Equal(command.ChannelId, feed.Channel.ChannelId);

            var channel = channelRepository[command.ChannelId!];
            Assert.NotNull(channel);
            Assert.Equal(command.ChannelId, channel.ChannelId);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenExistingFeedId_ShouldThrowException(CreateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper();
            var feed = mapper.Map<Feed>(command);
            var feedRepository = new MockedFeedRepository(new Dictionary<string, Feed>(capacity: 1) 
            { 
                { feed.FeedId, feed} 
            });

            var app = new CreateFeedApp(
                feedRepository,
                mapper,
                new MockedChannelService());

            var exception = await Assert.ThrowsAsync<FeedAlreadyExistingException>(() =>  app.CreateFeedAsync(command));

            Assert.Equal(command.FeedId, exception.FeedId);
        }
    }
}
