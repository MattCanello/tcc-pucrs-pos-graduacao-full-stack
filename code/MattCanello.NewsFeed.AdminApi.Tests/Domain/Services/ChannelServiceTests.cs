using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Services;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;
using MattCanello.NewsFeed.AdminApi.Tests.Mocks;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Services
{
    public class ChannelServiceTests
    {
        [Fact]
        public async Task GetOrCreateAsync_GivenNullChannelId_ShouldThrowException()
        {
            var service = new ChannelService(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetOrCreateAsync(null!));

            Assert.Equal("channelId", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task GetOrCreateAsync_GivenNewChannel_ShouldCreateChannel(string channelId)
        {
            var service = new ChannelService(
                new MockedChannelRepository(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var channel = await service.GetOrCreateAsync(channelId);

            Assert.NotNull(channel);
            Assert.Equal(channelId, channel.ChannelId);
        }

        [Theory, AutoData]
        public async Task GetOrCreateAsync_GivenExistingChannel_ShouldReturnChannel(Channel channel)
        {
            var service = new ChannelService(
                new MockedChannelRepository(channel),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var resultingChannel = await service.GetOrCreateAsync(channel.ChannelId);

            Assert.NotNull(resultingChannel);
            Assert.Equal(channel, resultingChannel);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenNullChannelId_ShouldThrowException(RssChannel data)
        {
            var service = new ChannelService(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateChannelAsync(null!, data));

            Assert.Equal("channelId", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenNullData_ShouldThrowException(string channelId)
        {
            var service = new ChannelService(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateChannelAsync(channelId, null!));

            Assert.Equal("rssChannel", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenNewChannel_ShouldCreateChannel(string channelId, RssChannel data)
        {
            var service = new ChannelService(
                new MockedChannelRepository(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var channel = await service.UpdateChannelAsync(channelId, data);

            Assert.NotNull(channel);
            Assert.Equal(channelId, channel.ChannelId);
            Assert.Equal(data.Name, channel.Name);
            Assert.Equal(data.ImageUrl, channel.ImageUrl);
            Assert.Equal(data.Url, channel.Url);
            Assert.Equal(data.Copyright, channel.Copyright);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenExistingChannel_ShouldUpdateChannel(string channelId, RssChannel data)
        {
            var mapping = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
            var repository = new MockedChannelRepository(mapping.Map<Channel>(data) with { ChannelId = channelId });

            var service = new ChannelService(
                repository,
                mapping);

            var channel = await service.UpdateChannelAsync(channelId, data);

            Assert.NotNull(channel);
            Assert.Equal(channelId, channel.ChannelId);
            Assert.Equal(data.Name, channel.Name);
            Assert.Equal(data.ImageUrl, channel.ImageUrl);
            Assert.Equal(data.Url, channel.Url);
            Assert.Equal(data.Copyright, channel.Copyright);
            Assert.Equal(1, repository.Count);
        }
    }
}
