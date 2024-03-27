﻿using AutoFixture.Xunit2;
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
                MockedChannelServiceBuilder.CreateInstance());

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.Equal(command.FeedId, feed.FeedId);
            Assert.Equal(command.Url, feed.Url);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenNewChannelWithoutChannelData_ShouldCreateSimpleChannel(CreateFeedCommand command)
        {
            command.Channel = null;
            var channelRepository = new MockedChannelRepository();

            var app = new CreateFeedApp(
                new MockedFeedRepository(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper(),
                MockedChannelServiceBuilder.CreateInstance(channelRepository));

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.NotNull(feed.Channel);
            Assert.Equal(command.ChannelId, feed.Channel.ChannelId);

            var channel = channelRepository[command.ChannelId!];
            Assert.NotNull(channel);

            Assert.Equal(command.ChannelId, channel.ChannelId);
            Assert.Null(channel.Copyright);
            Assert.Null(channel.ImageUrl);
            Assert.Null(channel.Language);
            Assert.Null(channel.Name);
            Assert.Null(channel.Url);

            // TODO: Adicionar verificação para CreatedDate
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenNewChannelWithChannelData_ShouldCreateSimpleChannel(CreateFeedCommand command)
        {
            var channelRepository = new MockedChannelRepository();

            var app = new CreateFeedApp(
                new MockedFeedRepository(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper(),
                MockedChannelServiceBuilder.CreateInstance(channelRepository));

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.NotNull(feed.Channel);
            Assert.Equal(command.ChannelId, feed.Channel.ChannelId);

            var channel = channelRepository[command.ChannelId!];
            Assert.NotNull(channel);

            Assert.NotNull(command.Channel);
            Assert.Equal(command.ChannelId, channel.ChannelId);
            Assert.Equal(command.Channel.Copyright, channel.Copyright);
            Assert.Equal(command.Channel.ImageUrl, channel.ImageUrl);
            Assert.Equal(command.Channel.Language, channel.Language);
            Assert.Equal(command.Channel.Name, channel.Name);
            Assert.Equal(command.Channel.Url, channel.Url);

            // TODO: Adicionar verificação para CreatedDate
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
                MockedChannelServiceBuilder.CreateInstance());

            var exception = await Assert.ThrowsAsync<FeedAlreadyExistingException>(() =>  app.CreateFeedAsync(command));

            Assert.Equal(command.FeedId, exception.FeedId);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenPreExistingChannelWithoutChannelData_ShouldKeepChannelData(Channel channel, CreateFeedCommand command)
        {
            command.Channel = null;
            command.ChannelId = channel.ChannelId;
            var channelRepository = new MockedChannelRepository(new Dictionary<string, Channel>(capacity: 1) { { channel.ChannelId, channel } });

            var app = new CreateFeedApp(
                new MockedFeedRepository(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper(),
                MockedChannelServiceBuilder.CreateInstance(channelRepository));

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.NotNull(feed.Channel);
            Assert.Equal(command.ChannelId, feed.Channel.ChannelId);

            var resultingChannel = channelRepository[command.ChannelId!];
            Assert.NotNull(resultingChannel);

            Assert.Equal(channel.ChannelId, resultingChannel.ChannelId);
            Assert.Equal(channel.Copyright, resultingChannel.Copyright);
            Assert.Equal(channel.ImageUrl, resultingChannel.ImageUrl);
            Assert.Equal(channel.Language, resultingChannel.Language);
            Assert.Equal(channel.Name, resultingChannel.Name);
            Assert.Equal(channel.Url, resultingChannel.Url);

            // TODO: Adicionar verificação para CreatedDate
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenPreExistingChannelWithChannelData_ShouldKeepChannelData(Channel channel, CreateFeedCommand command)
        {
            command.ChannelId = channel.ChannelId;
            var channelRepository = new MockedChannelRepository(new Dictionary<string, Channel>(capacity: 1) { { channel.ChannelId, channel } });

            var app = new CreateFeedApp(
                new MockedFeedRepository(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper(),
                MockedChannelServiceBuilder.CreateInstance(channelRepository));

            var feed = await app.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.NotNull(feed.Channel);
            Assert.Equal(command.ChannelId, feed.Channel.ChannelId);

            var resultingChannel = channelRepository[command.ChannelId!];
            Assert.NotNull(resultingChannel);

            Assert.Equal(channel.ChannelId, resultingChannel.ChannelId);
            Assert.Equal(channel.Copyright, resultingChannel.Copyright);
            Assert.Equal(channel.ImageUrl, resultingChannel.ImageUrl);
            Assert.Equal(channel.Language, resultingChannel.Language);
            Assert.Equal(channel.Name, resultingChannel.Name);
            Assert.Equal(channel.Url, resultingChannel.Url);

            // TODO: Adicionar verificação para CreatedDate
        }
    }
}
