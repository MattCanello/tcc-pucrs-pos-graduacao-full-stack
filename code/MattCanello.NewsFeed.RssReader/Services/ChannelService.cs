using MattCanello.NewsFeed.RssReader.Interfaces;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class ChannelService : IChannelService
    {
        private readonly IChannelReader _channelReader;
        private readonly IChannelPublisher _channelPublisher;

        public ChannelService(IChannelReader channelReader, IChannelPublisher channelPublisher)
        {
            _channelReader = channelReader;
            _channelPublisher = channelPublisher;
        }

        public async Task ProcessChannelUpdateFromRssAsync(SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            var channel = _channelReader.FromRSS(syndicationFeed);

            if (channel is null)
                return;

            await _channelPublisher.PublishChannelUpdatedAsync(channel, cancellationToken);
        }
    }
}
