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
        public async Task AppendDataToChannelAsync_GivenNullChannelId_ShouldThrowException(RssData data)
        {
            var service = new ChannelService(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.AppendDataToChannelAsync(null!, data));

            Assert.Equal("channelId", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task AppendDataToChannelAsync_GivenNullData_ShouldThrowException(string channelId)
        {
            var service = new ChannelService(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.AppendDataToChannelAsync(channelId, null!));

            Assert.Equal("data", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task AppendDataToChannelAsync_GivenNewChannel_ShouldCreateChannel(string channelId, RssData data)
        {
            var service = new ChannelService(
                new MockedChannelRepository(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());

            var channel = await service.AppendDataToChannelAsync(channelId, data);

            Assert.NotNull(channel);
            Assert.Equal(channelId, channel.ChannelId);
            Assert.Equal(data.Name, channel.Name);
            Assert.Equal(data.Language, channel.Language);
            Assert.Equal(data.ImageUrl, channel.ImageUrl);
            Assert.Equal(data.Url, channel.Url);
            Assert.Equal(data.Copyright, channel.Copyright);
        }

        [Theory, AutoData]
        public async Task AppendDataToChannelAsync_GivenExistingChannel_ShouldUpdateChannel(string channelId, RssData data)
        {
            var mapping = new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();

            var repository = new MockedChannelRepository(new Channel()
            {
                ChannelId = channelId,
                Copyright = data.Copyright,
                ImageUrl = data.ImageUrl,
                Language = data.Language,
                Name = data.Name,
                Url = data.Url
            });

            var service = new ChannelService(
                repository,
                mapping);

            var channel = await service.AppendDataToChannelAsync(channelId, data);

            Assert.NotNull(channel);
            Assert.Equal(channelId, channel.ChannelId);
            Assert.Equal(data.Name, channel.Name);
            Assert.Equal(data.Language, channel.Language);
            Assert.Equal(data.ImageUrl, channel.ImageUrl);
            Assert.Equal(data.Url, channel.Url);
            Assert.Equal(data.Copyright, channel.Copyright);
            Assert.Equal(1, repository.Count);
        }
    }
}
