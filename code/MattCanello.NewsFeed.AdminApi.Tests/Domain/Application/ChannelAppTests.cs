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
    public class ChannelAppTests
    {
        [Fact]
        public async Task CreateAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new ChannelApp(null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.CreateAsync(null!));

            Assert.NotNull(exception);
            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task CreateAsync_GivenExistingChannel_ShouldThrowExcpetion(Channel channel, CreateChannelCommand command)
        {
            command.ChannelId = channel.ChannelId;
            var app = new ChannelApp(
                new MockedChannelRepository(channel),
                new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var exception = await Assert.ThrowsAsync<ChannelAlreadyExistsException>(() => app.CreateAsync(command));

            Assert.NotNull(exception);
            Assert.Equal(command.ChannelId, exception.ChannelId);
        }

        [Theory, AutoData]
        public async Task CreateAsync_GivenNewChannel_ShouldCreateChannel(CreateChannelCommand command, DateTimeOffset now)
        {
            var repository = new MockedChannelRepository();

            var app = new ChannelApp(
                repository,
                new MockedDateTimeProvider(now),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var channel = await app.CreateAsync(command);

            Assert.NotNull(channel);
            Assert.Equal(command.ChannelId, channel.ChannelId);

            Assert.NotNull(command.Data);
            Assert.Equal(command.Data.Name, channel.Name);
            Assert.Equal(command.Data.ImageUrl, channel.ImageUrl);
            Assert.Equal(command.Data.Url, channel.Url);
            Assert.Equal(command.Data.Copyright, channel.Copyright);

            Assert.Equal(1, repository.Count);
            Assert.Equal(now, channel.CreatedAt);
        }

        [Fact]
        public async Task UpdateAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new ChannelApp(null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.UpdateAsync(null!));

            Assert.NotNull(exception);
            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_GivenNotExistingChannel_ShouldThrowExcpetion(UpdateChannelCommand command)
        {
            var app = new ChannelApp(
                new MockedChannelRepository(),
                new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var exception = await Assert.ThrowsAsync<ChannelNotFoundException>(() => app.UpdateAsync(command));

            Assert.NotNull(exception);
            Assert.Equal(command.ChannelId, exception.ChannelId);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_GivenExistingChannel_ShouldOverrideProperties(Channel channel, UpdateChannelCommand command)
        {
            command.ChannelId = channel.ChannelId;
            var repository = new MockedChannelRepository(channel);

            var app = new ChannelApp(
                repository,
                new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var updatedChannel = await app.UpdateAsync(command);

            Assert.NotNull(updatedChannel);
            Assert.Equal(command.ChannelId, updatedChannel.ChannelId);

            Assert.NotNull(command.Data);
            Assert.Equal(command.Data.Name, updatedChannel.Name);
            Assert.Equal(command.Data.ImageUrl, updatedChannel.ImageUrl);
            Assert.Equal(command.Data.Url, updatedChannel.Url);
            Assert.Equal(command.Data.Copyright, updatedChannel.Copyright);
            Assert.Equal(channel.CreatedAt, updatedChannel.CreatedAt);

            Assert.Equal(1, repository.Count);
        }
    }
}
