﻿using AutoMapper;
using MattCanello.NewsFeed.RssReader.Domain.Exceptions;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Clients;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Repositories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services;
using MattCanello.NewsFeed.RssReader.Domain.Messages;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Application
{
    public sealed class RssApp : IRssApp
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IMapper _mapper;
        private readonly IRssClient _rssClient;

        private readonly IChannelService _channelService;
        private readonly IEntryService _entryService;

        public RssApp(IFeedRepository feedRepository, IMapper mapper, IRssClient rssClient, IChannelService channelService, IEntryService entryService)
        {
            _feedRepository = feedRepository;
            _mapper = mapper;
            _rssClient = rssClient;
            _channelService = channelService;
            _entryService = entryService;
        }

        public async Task ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var feed = await _feedRepository.GetAsync(feedId, cancellationToken)
                ?? throw new FeedNotFoundException(feedId);

            var requestMessage = _mapper
                .Map<ReadRssRequestMessage>(feed);

            var response = await _rssClient
                .ReadAsync(requestMessage, cancellationToken);

            if (response.IsNotModified || response.Feed is null)
                return;

            var responseDate = response.ResponseDate ?? DateTimeOffset.UtcNow;

            await _entryService.ProcessEntriesFromRSSAsync(feedId, response.Feed, cancellationToken);

            var feedConsumedTask = _channelService.ProcessFeedConsumedAsync(feedId, responseDate, response.Feed, cancellationToken);
            var updateFeedTask = UpdateFeedFromResponseAsync(feed, response, cancellationToken);

            await Task.WhenAll(feedConsumedTask, updateFeedTask);
        }

        private async Task UpdateFeedFromResponseAsync(Feed feed, ReadRssResponseMessage response, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feed);
            ArgumentNullException.ThrowIfNull(response);

            feed.SetAsModified(response.ETag, response.ResponseDate);
            await _feedRepository.UpdateAsync(feed.FeedId, feed, cancellationToken);
        }
    }
}
