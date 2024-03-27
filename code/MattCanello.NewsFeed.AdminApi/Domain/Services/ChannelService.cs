using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

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

        public async Task<Channel> GetOrCreateAsync(string channelId, RssData? data = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            if (channel is not null)
                return channel;

            channel = (_mapper.Map<Channel>(data) ?? new Channel())
                with { ChannelId = channelId, CreatedAt = DateTimeOffset.UtcNow };

            channel = await _channelRepository.CreateAsync(channel, cancellationToken);

            return channel;
        }

        public async Task<Channel> AppendDataToChannelAsync(string channelId, RssData data, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);
            ArgumentNullException.ThrowIfNull(data);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken) 
                ?? new Channel { ChannelId = channelId, CreatedAt = DateTimeOffset.UtcNow };

            channel = _mapper.Map(data, channel);

            channel = await _channelRepository.UpdateAsync(channel, cancellationToken);

            return channel;
        }
    }
}
