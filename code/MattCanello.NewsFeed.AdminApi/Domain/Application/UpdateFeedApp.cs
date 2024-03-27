using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Application
{
    public sealed class UpdateFeedApp : IUpdateFeedApp
    {
        private readonly IChannelService _channelService;
        private readonly IFeedService _feedService;

        public UpdateFeedApp(IChannelService channelService, IFeedService feedService)
        {
            _channelService = channelService;
            _feedService = feedService;
        }

        public async Task<Feed> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var feed = await _feedService.UpdateFeedAsync(command.FeedId!, command.Data!, cancellationToken);

            if (feed.Channel != null)
                feed.Channel = await _channelService.AppendDataToChannelAsync(feed.Channel.ChannelId, command.Data!, cancellationToken);

            return feed;
        }
    }
}
