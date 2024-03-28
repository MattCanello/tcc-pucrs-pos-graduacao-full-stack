using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;

namespace MattCanello.NewsFeed.AdminApi.Domain.Services
{
    public sealed class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public ChannelService(IChannelRepository channelRepository, IDateTimeProvider dateTimeProvider, IMapper mapper)
        {
            _channelRepository = channelRepository;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }

        public async Task<Channel> GetOrCreateAsync(string channelId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            if (channel is not null)
                return channel;

            channel = new Channel() { ChannelId = channelId, CreatedAt = _dateTimeProvider.GetUtcNow() };

            channel = await _channelRepository.CreateAsync(channel, cancellationToken);

            return channel;
        }

        public async Task<Channel> UpdateChannelAsync(string channelId, RssChannel rssChannel, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);
            ArgumentNullException.ThrowIfNull(rssChannel);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken) 
                ?? new Channel { ChannelId = channelId, CreatedAt = _dateTimeProvider.GetUtcNow() };

            channel = _mapper.Map(rssChannel, channel);

            channel = await _channelRepository.UpdateAsync(channel, cancellationToken);

            return channel;
        }
    }
}
