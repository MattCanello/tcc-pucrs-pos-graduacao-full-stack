﻿using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Services
{
    public sealed class ChannelService : IChannelService
    {
        private readonly IChannelFactory _channelFactory;
        private readonly IChannelUpdatedPublisher _channelUpdatedPublisher;

        public ChannelService(IChannelFactory channelFactory, IChannelUpdatedPublisher channelUpdatedPublisher)
        {
            _channelFactory = channelFactory;
            _channelUpdatedPublisher = channelUpdatedPublisher;
        }

        public async Task ProcessChannelUpdateFromRssAsync(string feedId, SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            var channel = _channelFactory.FromRSS(syndicationFeed);

            if (channel is null)
                return;

            await _channelUpdatedPublisher.PublishAsync(feedId, channel, cancellationToken);
        }
    }
}
