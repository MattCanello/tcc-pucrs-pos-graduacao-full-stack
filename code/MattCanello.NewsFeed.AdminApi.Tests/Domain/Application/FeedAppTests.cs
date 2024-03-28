using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Application;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;
using MattCanello.NewsFeed.AdminApi.Tests.Mocks;
using MattCanello.NewsFeed.Cross.Abstractions.Tests.Mocks;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Application
{
    public class FeedAppTests
    {
        [Fact]
        public async Task CreateFeedAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new FeedApp(null!, null!, null!, null!);

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
                new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.Equal(command.FeedId, feed.FeedId);
            Assert.Equal(command.Url, feed.Url);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenNewChannel_ShouldCreateSimpleChannel(CreateFeedCommand command, DateTimeOffset now)
        {
            var channelRepository = new MockedChannelRepository();
            var dateTimeProvider = new MockedDateTimeProvider(now);

            var app = new FeedApp(
                new MockedFeedRepository(),
                MockedChannelServiceBuilder.CreateInstance(channelRepository, dateTimeProvider),
                dateTimeProvider,
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

            Assert.Equal(now, channel.CreatedAt);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenExistingFeedId_ShouldThrowException(CreateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper();
            var feed = mapper.Map<FeedWithChannel>(command);
            var feedRepository = new MockedFeedRepository(feed);

            var app = new FeedApp(
                feedRepository,
                MockedChannelServiceBuilder.CreateInstance(),
                new MockedDateTimeProvider(),
                mapper);

            var exception = await Assert.ThrowsAsync<FeedAlreadyExistsException>(() => app.CreateFeedAsync(command));

            Assert.Equal(command.FeedId, exception.FeedId);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenPreExistingChannel_ShouldKeepChannelInfo(Channel channel, CreateFeedCommand command, DateTimeOffset now)
        {
            command.ChannelId = channel.ChannelId;
            var channelRepository = new MockedChannelRepository(channel);
            var dateTimeProvider = new MockedDateTimeProvider(now);

            var app = new FeedApp(
                new MockedFeedRepository(),
                MockedChannelServiceBuilder.CreateInstance(channelRepository, dateTimeProvider),
                dateTimeProvider,
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
            Assert.Equal(channel.CreatedAt, resultingChannel.CreatedAt);
        }

        [Fact]
        public async Task UpdateFeedAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new FeedApp(null!, null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.UpdateFeedAsync(null!));

            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenUnknownFeedId_ShouldThrowException(UpdateFeedCommand command)
        {
            var app = new FeedApp(
                new MockedFeedRepository(),
                MockedChannelServiceBuilder.CreateInstance(),
                new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var exception = await Assert.ThrowsAsync<FeedNotFoundException>(() => app.UpdateFeedAsync(command));

            Assert.Equal(exception.FeedId, command.FeedId);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenUpdatedDataOnEmptyFeed_ShouldUpdateFeed(string channelId, string feedUrl, UpdateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
            var feed = new FeedWithChannel() { FeedId = command.FeedId!, Url = feedUrl, Channel = mapper.Map<Channel>(command.Channel) with { ChannelId = channelId } };

            var app = new FeedApp(
                new MockedFeedRepository(feed), 
                MockedChannelServiceBuilder.CreateInstance(),
                new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.NotNull(command.Channel);
            Assert.Equal(command.Channel.Name, updatedFeed.Name);
            Assert.Equal(command.Channel.Language, updatedFeed.Language);
            Assert.Equal(command.Channel.Copyright, updatedFeed.Copyright);
            Assert.Equal(command.Channel.ImageUrl, updatedFeed.ImageUrl);
            Assert.Equal(feedUrl, updatedFeed.Url);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenUpdatedDataOnFilledFeed_ShouldNotUpdateFeed(FeedWithChannel feed, UpdateFeedCommand command)
        {
            command.FeedId = feed.FeedId;

            var app = new FeedApp(
                new MockedFeedRepository(feed),
                MockedChannelServiceBuilder.CreateInstance(),
                new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.Equal(feed.Name, updatedFeed.Name);
            Assert.Equal(feed.Language, updatedFeed.Language);
            Assert.Equal(feed.Copyright, updatedFeed.Copyright);
            Assert.Equal(feed.ImageUrl, updatedFeed.ImageUrl);
            Assert.Equal(feed.Url, updatedFeed.Url);
            Assert.Equal(feed.CreatedAt, updatedFeed.CreatedAt);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenNewChannel_ShouldCreateChannel(string channelId, UpdateFeedCommand command, DateTimeOffset now)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
            var feed = new FeedWithChannel() { FeedId = command.FeedId!, Channel = mapper.Map<Channel>(command.Channel) with { ChannelId = channelId } };
            var dateTimeProvider = new MockedDateTimeProvider(now);

            var app = new FeedApp(
                new MockedFeedRepository(feed),
                MockedChannelServiceBuilder.CreateInstance(dateTimeProvider: dateTimeProvider),
                dateTimeProvider,
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.NotNull(updatedFeed.Channel);

            Assert.NotNull(feed.Channel);
            Assert.Equal(feed.Channel.ChannelId, updatedFeed.Channel.ChannelId);

            Assert.NotNull(command.Channel);
            Assert.Equal(command.Channel.Name, updatedFeed.Channel.Name);
            Assert.Equal(command.Channel.ImageUrl, updatedFeed.Channel.ImageUrl);
            Assert.Equal(command.Channel.Url, updatedFeed.Channel.Url);
            Assert.Equal(command.Channel.Copyright, updatedFeed.Channel.Copyright);

            Assert.Equal(now, updatedFeed.Channel.CreatedAt);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenExistingEmptyChannel_ShouldUpdateChannel(string channelId, UpdateFeedCommand command)
        {
            var feed = new FeedWithChannel() { FeedId = command.FeedId!, Channel = new Channel() { ChannelId = channelId } };

            var app = new FeedApp(
                new MockedFeedRepository(feed),
                MockedChannelServiceBuilder.CreateInstance(new MockedChannelRepository(feed.Channel)),
                new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());

            var updatedFeed = await app.UpdateFeedAsync(command);

            Assert.NotNull(updatedFeed);
            Assert.NotNull(updatedFeed.Channel);

            Assert.NotNull(feed.Channel);
            Assert.Equal(feed.Channel.ChannelId, updatedFeed.Channel.ChannelId);

            Assert.NotNull(command.Channel);
            Assert.Equal(command.Channel.Name, updatedFeed.Channel.Name);
            Assert.Equal(command.Channel.ImageUrl, updatedFeed.Channel.ImageUrl);
            Assert.Equal(command.Channel.Url, updatedFeed.Channel.Url);
            Assert.Equal(command.Channel.Copyright, updatedFeed.Channel.Copyright);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenExistingFilledChannel_ShouldNotUpdateChannel(string channelId, UpdateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
            var channel = mapper.Map<Channel>(command.Channel) with { ChannelId = channelId };
            var feed = new FeedWithChannel() { FeedId = command.FeedId!, Channel = channel };

            var app = new FeedApp(
                new MockedFeedRepository(feed),
               MockedChannelServiceBuilder.CreateInstance(new MockedChannelRepository(channel)),
               new MockedDateTimeProvider(),
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
            Assert.Equal(channel.CreatedAt, updatedFeed.Channel.CreatedAt);
        }
    }
}
