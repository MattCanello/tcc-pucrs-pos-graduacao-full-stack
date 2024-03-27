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
    public class FeedAppTests
    {
        [Fact]
        public async Task CreateFeedAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new FeedApp(null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.CreateFeedAsync(null!));

            Assert.NotNull(exception);
            Assert.Equal("createFeedCommand", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenNewFeed_ShouldCreateFeed(CreateFeedCommand command)
        {
            var app = new FeedApp(
                new MockedFeedRepository(),
                MockedChannelServiceBuilder.CreateInstance(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.Equal(command.FeedId, feed.FeedId);
            Assert.Equal(command.Url, feed.Url);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenNewChannel_ShouldCreateSimpleChannel(CreateFeedCommand command)
        {
            var channelRepository = new MockedChannelRepository();

            var app = new FeedApp(
                new MockedFeedRepository(),
                MockedChannelServiceBuilder.CreateInstance(channelRepository),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.NotNull(feed.Channel);
            Assert.Equal(command.ChannelId, feed.Channel.ChannelId);

            var channel = channelRepository[command.ChannelId!];
            Assert.NotNull(channel);

            Assert.Equal(command.ChannelId, channel.ChannelId);
            Assert.Null(channel.Copyright);
            Assert.Null(channel.ImageUrl);
            Assert.Null(channel.Name);
            Assert.Null(channel.Url);

            // TODO: Adicionar verificação para CreatedDate
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenExistingFeedId_ShouldThrowException(CreateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper();
            var feed = mapper.Map<Feed>(command);
            var feedRepository = new MockedFeedRepository(feed);

            var app = new FeedApp(
                feedRepository,
                MockedChannelServiceBuilder.CreateInstance(),
                mapper);

            var exception = await Assert.ThrowsAsync<FeedAlreadyExistsException>(() => app.CreateFeedAsync(command));

            Assert.Equal(command.FeedId, exception.FeedId);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenPreExistingChannel_ShouldKeepChannelInfo(Channel channel, CreateFeedCommand command)
        {
            command.ChannelId = channel.ChannelId;
            var channelRepository = new MockedChannelRepository(channel);

            var app = new FeedApp(
                new MockedFeedRepository(),
                MockedChannelServiceBuilder.CreateInstance(channelRepository),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.NotNull(feed.Channel);
            Assert.Equal(command.ChannelId, feed.Channel.ChannelId);

            var resultingChannel = channelRepository[command.ChannelId!];
            Assert.NotNull(resultingChannel);

            Assert.Equal(channel.ChannelId, resultingChannel.ChannelId);
            Assert.Equal(channel.Copyright, resultingChannel.Copyright);
            Assert.Equal(channel.ImageUrl, resultingChannel.ImageUrl);
            Assert.Equal(channel.Name, resultingChannel.Name);
            Assert.Equal(channel.Url, resultingChannel.Url);

            // TODO: Adicionar verificação para CreatedDate
        }

        [Fact]
        public async Task UpdateFeedAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new FeedApp(null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.UpdateFeedAsync(null!));

            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenUnknownFeedId_ShouldThrowException(UpdateFeedCommand command)
        {
            var app = new FeedApp(
                new MockedFeedRepository(),
                MockedChannelServiceBuilder.CreateInstance(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var exception = await Assert.ThrowsAsync<FeedNotFoundException>(() => app.UpdateFeedAsync(command));

            Assert.Equal(exception.FeedId, command.FeedId);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenUpdatedDataOnEmptyFeed_ShouldUpdateFeed(string channelId, string feedUrl, UpdateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
            var feed = new Feed() { FeedId = command.FeedId!, Url = feedUrl, Channel = mapper.Map<Channel>(command.Data) with { ChannelId = channelId } };

            var app = new FeedApp(
                new MockedFeedRepository(feed), 
                MockedChannelServiceBuilder.CreateInstance(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.NotNull(command.Data);
            Assert.Equal(command.Data.Name, updatedFeed.Name);
            Assert.Equal(command.Data.Language, updatedFeed.Language);
            Assert.Equal(command.Data.Copyright, updatedFeed.Copyright);
            Assert.Equal(command.Data.ImageUrl, updatedFeed.ImageUrl);
            Assert.Equal(feedUrl, updatedFeed.Url);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenUpdatedDataOnFilledFeed_ShouldNotUpdateFeed(Feed feed, UpdateFeedCommand command)
        {
            command.FeedId = feed.FeedId;

            var app = new FeedApp(
                new MockedFeedRepository(feed),
                MockedChannelServiceBuilder.CreateInstance(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.Equal(feed.Name, updatedFeed.Name);
            Assert.Equal(feed.Language, updatedFeed.Language);
            Assert.Equal(feed.Copyright, updatedFeed.Copyright);
            Assert.Equal(feed.ImageUrl, updatedFeed.ImageUrl);
            Assert.Equal(feed.Url, updatedFeed.Url);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenNewChannel_ShouldCreateChannel(string channelId, UpdateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
            var feed = new Feed() { FeedId = command.FeedId!, Channel = mapper.Map<Channel>(command.Data) with { ChannelId = channelId } };

            var app = new FeedApp(
                new MockedFeedRepository(feed),
                MockedChannelServiceBuilder.CreateInstance(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.NotNull(updatedFeed.Channel);

            Assert.NotNull(feed.Channel);
            Assert.Equal(feed.Channel.ChannelId, updatedFeed.Channel.ChannelId);

            Assert.NotNull(command.Data);
            Assert.Equal(command.Data.Name, updatedFeed.Channel.Name);
            Assert.Equal(command.Data.ImageUrl, updatedFeed.Channel.ImageUrl);
            Assert.Equal(command.Data.Url, updatedFeed.Channel.Url);
            Assert.Equal(command.Data.Copyright, updatedFeed.Channel.Copyright);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenExistingEmptyChannel_ShouldUpdateChannel(string channelId, UpdateFeedCommand command)
        {
            var feed = new Feed() { FeedId = command.FeedId!, Channel = new Channel() { ChannelId = channelId } };

            var app = new FeedApp(
                new MockedFeedRepository(feed),
                MockedChannelServiceBuilder.CreateInstance(new MockedChannelRepository(feed.Channel)),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.NotNull(updatedFeed.Channel);

            Assert.NotNull(feed.Channel);
            Assert.Equal(feed.Channel.ChannelId, updatedFeed.Channel.ChannelId);

            Assert.NotNull(command.Data);
            Assert.Equal(command.Data.Name, updatedFeed.Channel.Name);
            Assert.Equal(command.Data.ImageUrl, updatedFeed.Channel.ImageUrl);
            Assert.Equal(command.Data.Url, updatedFeed.Channel.Url);
            Assert.Equal(command.Data.Copyright, updatedFeed.Channel.Copyright);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenExistingFilledChannel_ShouldNotUpdateChannel(string channelId, UpdateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
            var channel = mapper.Map<Channel>(command.Data) with { ChannelId = channelId };
            var feed = new Feed() { FeedId = command.FeedId!, Channel = channel };

            var app = new FeedApp(
                new MockedFeedRepository(feed),
               MockedChannelServiceBuilder.CreateInstance(new MockedChannelRepository(channel)),
               new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.NotNull(updatedFeed.Channel);

            Assert.NotNull(feed.Channel);
            Assert.Equal(feed.Channel.ChannelId, updatedFeed.Channel.ChannelId);

            Assert.Equal(channel.Name, updatedFeed.Channel.Name);
            Assert.Equal(channel.ImageUrl, updatedFeed.Channel.ImageUrl);
            Assert.Equal(channel.Url, updatedFeed.Channel.Url);
            Assert.Equal(channel.Copyright, updatedFeed.Channel.Copyright);
        }
    }
}
