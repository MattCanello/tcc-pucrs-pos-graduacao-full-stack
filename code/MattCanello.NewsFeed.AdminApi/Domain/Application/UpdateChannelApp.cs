using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Application
{
    public sealed class UpdateChannelApp : IUpdateChannelApp
    {
        private readonly IChannelService _channelService;
        private readonly IFeedRepository _feedRepository;

        public UpdateChannelApp(IChannelService channelService, IFeedRepository feedRepository)
        {
            _channelService = channelService;
            _feedRepository = feedRepository;
        }

        public async Task<Channel> UpdateChannelAsync(UpdateChannelCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var feed = await _feedRepository.GetByIdAsync(command.FeedId!,cancellationToken) 
                ?? throw new FeedNotFoundException(command.FeedId!);

            var channel = await _channelService.UpdateChannelAsync(feed.Channel!.ChannelId, command.Channel!, cancellationToken);

            return channel;
        }
    }
}
