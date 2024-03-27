using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Application
{
    public sealed class ChannelApp : IChannelApp
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;

        public ChannelApp(IChannelRepository channelRepository, IMapper mapper)
        {
            _channelRepository = channelRepository;
            _mapper = mapper;
        }

        public async Task<Channel> CreateAsync(CreateChannelCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            await CheckIfChannelExistsAsync(command.ChannelId!, cancellationToken);

            var channel = _mapper.Map<Channel>(command);

            channel = await _channelRepository.CreateAsync(channel, cancellationToken);

            return channel;
        }

        public async Task<Channel> UpdateAsync(UpdateChannelCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var channel = await _channelRepository.GetByIdAsync(command.ChannelId!, cancellationToken) 
                ?? throw new ChannelNotFoundException(command.ChannelId!);

            channel = _mapper.Map(command, channel);

            channel = await _channelRepository.UpdateAsync(channel, cancellationToken);

            return channel;
        }

        private async Task CheckIfChannelExistsAsync(string channelId, CancellationToken cancellationToken = default)
        {
            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            if (channel is null)
                return;

            throw new ChannelAlreadyExistsException(channelId);
        }
    }
}
