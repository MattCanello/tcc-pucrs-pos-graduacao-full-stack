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
    public class UpdateFeedAppTests
    {
        [Fact]
        public async Task UpdateFeedAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new UpdateFeedApp(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.UpdateFeedAsync(null!));

            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenUnknownFeedId_ShouldThrowException(UpdateFeedCommand command)
        {
            var app = new UpdateFeedApp(
                MockedChannelServiceBuilder.CreateInstance(),
                MockedFeedServiceBuilder.CreateInstance());

            var exception = await Assert.ThrowsAsync<FeedNotFoundException>(() => app.UpdateFeedAsync(command));

            Assert.Equal(exception.FeedId, command.FeedId);
        }

        [Theory, AutoData]
        public async Task UpdateFeedAsync_GivenUpdatedDataOnEmptyFeed_ShouldUpdateFeed(string channelId, string feedUrl, UpdateFeedCommand command)
        {
            var mapper = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
            var feed = new Feed() { FeedId = command.FeedId!, Url = feedUrl, Channel = mapper.Map<Channel>(command.Data) with { ChannelId = channelId } };

            var app = new UpdateFeedApp(
               MockedChannelServiceBuilder.CreateInstance(),
               MockedFeedServiceBuilder.CreateInstance(new MockedFeedRepository(feed)));

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

            var app = new UpdateFeedApp(
               MockedChannelServiceBuilder.CreateInstance(),
               MockedFeedServiceBuilder.CreateInstance(new MockedFeedRepository(feed)));

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

            var app = new UpdateFeedApp(
               MockedChannelServiceBuilder.CreateInstance(),
               MockedFeedServiceBuilder.CreateInstance(new MockedFeedRepository(feed)));

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

        [Theory,AutoData]
        public async Task UpdateFeedAsync_GivenExistingEmptyChannel_ShouldUpdateChannel(string channelId, UpdateFeedCommand command)
        {
            var feed = new Feed() { FeedId = command.FeedId!, Channel = new Channel() { ChannelId = channelId } };
            var channelRepository = new MockedChannelRepository(feed.Channel);

            var app = new UpdateFeedApp(
               MockedChannelServiceBuilder.CreateInstance(channelRepository),
               MockedFeedServiceBuilder.CreateInstance(new MockedFeedRepository(feed)));

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
            var channelRepository = new MockedChannelRepository(channel);

            var app = new UpdateFeedApp(
               MockedChannelServiceBuilder.CreateInstance(channelRepository),
               MockedFeedServiceBuilder.CreateInstance(new MockedFeedRepository(feed)));

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
