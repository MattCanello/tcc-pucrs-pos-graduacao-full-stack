using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Services;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;
using MattCanello.NewsFeed.AdminApi.Tests.Mocks;
using static MattCanello.NewsFeed.AdminApi.Domain.Commands.UpdateChannelCommand;

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
                new MockedChannelRepository(new Dictionary<string, Channel>(capacity: 1)
                {
                    { channel.ChannelId, channel }
                }),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var resultingChannel = await service.GetOrCreateAsync(channel.ChannelId);

            Assert.NotNull(resultingChannel);
            Assert.Equal(channel, resultingChannel);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenNullChannelId_ShouldThrowException(ChannelData channelData)
        {
            var service = new ChannelService(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateChannelAsync(null!, channelData));

            Assert.Equal("channelId", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenNullChannelData_ShouldThrowException(string channelId)
        {
            var service = new ChannelService(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateChannelAsync(channelId, null!));

            Assert.Equal("channelData", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenNewChannel_ShouldCreateChannel(string channelId, ChannelData channelData)
        {
            var service = new ChannelService(
                new MockedChannelRepository(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var channel = await service.UpdateChannelAsync(channelId, channelData);

            Assert.NotNull(channel);
            Assert.Equal(channelId, channel.ChannelId);
            Assert.Equal(channelData.Name, channel.Name);
            Assert.Equal(channelData.Language, channel.Language);
            Assert.Equal(channelData.ImageUrl, channel.ImageUrl);
            Assert.Equal(channelData.Url, channel.Url);
            Assert.Equal(channelData.Copyright, channel.Copyright);
        }

        [Theory, AutoData]
        public async Task UpdateChannelAsync_GivenExistingChannel_ShouldUpdateChannel(string channelId, ChannelData channelData)
        {
            var mapping = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();

            var repository = new MockedChannelRepository(new Dictionary<string, Channel>(capacity: 1)
            {
                { channelId, mapping.Map(channelData, new Channel() { ChannelId = channelId }) }
            });

            var service = new ChannelService(
                repository,
                mapping);

            var channel = await service.UpdateChannelAsync(channelId, channelData);

            Assert.NotNull(channel);
            Assert.Equal(channelId, channel.ChannelId);
            Assert.Equal(channelData.Name, channel.Name);
            Assert.Equal(channelData.Language, channel.Language);
            Assert.Equal(channelData.ImageUrl, channel.ImageUrl);
            Assert.Equal(channelData.Url, channel.Url);
            Assert.Equal(channelData.Copyright, channel.Copyright);
            Assert.Equal(1, repository.Count);
        }
    }
}
