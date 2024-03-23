using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;

        public MockedChannelService(IChannelRepository? channelRepository = null, IMapper? mapper = null)
        {
            _channelRepository = channelRepository ?? new MockedChannelRepository();
            _mapper = mapper ?? new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper();
        }

        public async Task<Channel> GetOrCreateAsync(string channelId, CancellationToken cancellationToken = default)
        {
            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            channel ??= new Channel() {  ChannelId = channelId, CreatedAt = DateTimeOffset.UtcNow };

            await _channelRepository.CreateAsync(channel, cancellationToken);

            return channel;
        }

        public async Task<Channel> UpdateChannelAsync(string channelId, UpdateChannelCommand.ChannelData channelData, CancellationToken cancellationToken = default)
        {
            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            channel = _mapper.Map(channelData, channel) ?? new Channel() { ChannelId = channelId, CreatedAt = DateTimeOffset.UtcNow };
            channel.ChannelId = channelId;

            await _channelRepository.UpdateAsync(channel, cancellationToken);

            return channel;
        }
    }
}
