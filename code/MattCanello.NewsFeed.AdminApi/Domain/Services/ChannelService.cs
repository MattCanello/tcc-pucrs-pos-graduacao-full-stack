using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Services
{
    public sealed class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;

        public ChannelService(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        public async Task<Channel> GetOrCreateAsync(string channelId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(channelId);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            if (channel is not null)
                return channel;

            channel = new Channel() { ChannelId = channelId, CreatedAt = DateTimeOffset.UtcNow };

            channel = await _channelRepository.CreateAsync(channel, cancellationToken);

            return channel;
        }
    }
}
