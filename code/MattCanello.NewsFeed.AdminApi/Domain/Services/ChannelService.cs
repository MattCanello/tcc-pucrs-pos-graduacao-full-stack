using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using static MattCanello.NewsFeed.AdminApi.Domain.Commands.UpdateChannelCommand;

namespace MattCanello.NewsFeed.AdminApi.Domain.Services
{
    public sealed class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;

        public ChannelService(IChannelRepository channelRepository, IMapper mapper)
        {
            _channelRepository = channelRepository;
            _mapper = mapper;
        }

        public async Task<Channel> GetOrCreateAsync(string channelId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            if (channel is not null)
                return channel;

            channel = new Channel() { ChannelId = channelId, CreatedAt = DateTimeOffset.UtcNow };

            channel = await _channelRepository.CreateAsync(channel, cancellationToken);

            return channel;
        }

        public async Task<Channel> UpdateChannelAsync(string channelId, ChannelData channelData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);
            ArgumentNullException.ThrowIfNull(channelData);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken) ?? new Channel { ChannelId = channelId };

            channel = _mapper.Map(channelData, channel);

            channel = await _channelRepository.UpdateAsync(channel, cancellationToken);

            return channel;
        }
    }
}
