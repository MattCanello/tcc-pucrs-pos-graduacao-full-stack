﻿using AutoMapper;
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

        public async Task<Channel> GetOrCreateAsync(string channelId, ChannelData? channelData = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            if (channel is not null)
                return channel;

            channel = (_mapper.Map<Channel>(channelData) ?? new Channel())
                with { ChannelId = channelId, CreatedAt = DateTimeOffset.UtcNow };

            channel = await _channelRepository.CreateAsync(channel, cancellationToken);

            return channel;
        }

        public async Task<Channel> AppendDataToChannelAsync(string channelId, ChannelData channelData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);
            ArgumentNullException.ThrowIfNull(channelData);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken) 
                ?? new Channel { ChannelId = channelId, CreatedAt = DateTimeOffset.UtcNow };

            // TODO: Transformar em mapping com ValueResolver
            channel.Copyright ??= channelData.Copyright;
            channel.ImageUrl ??= channelData.ImageUrl;
            channel.Language ??= channelData.Language;
            channel.Name ??= channelData.Name;
            channel.Url ??= channelData.Url;

            channel = await _channelRepository.UpdateAsync(channel, cancellationToken);

            return channel;
        }
    }
}
