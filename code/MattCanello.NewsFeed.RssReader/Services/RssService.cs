using MattCanello.NewsFeed.RssReader.Exceptions;
using MattCanello.NewsFeed.RssReader.Factories;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Messages;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class RssService : IRssService
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ReadRssRequestMessageFactory _readRssRequestMessageFactory;
        private readonly IRssClient _rssClient;

        private readonly IChannelService _channelService;
        private readonly IEntryService _entryService;

        public RssService(IFeedRepository feedRepository, ReadRssRequestMessageFactory readRssRequestMessageFactory, IRssClient rssClient, IChannelService channelService, IEntryService entryService)
        {
            _feedRepository = feedRepository;
            _readRssRequestMessageFactory = readRssRequestMessageFactory;
            _rssClient = rssClient;
            _channelService = channelService;
            _entryService = entryService;
        }

        public async Task ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var feed = await _feedRepository.GetAsync(feedId, cancellationToken) 
                ?? throw new FeedNotFoundException(feedId);

            var requestMessage = _readRssRequestMessageFactory
                .FromFeed(feed);

            var response = await _rssClient
                .ReadAsync(requestMessage, cancellationToken);

            if (response.IsNotModified || response.Feed is null)
                return;

            var processChannelTask = _channelService.ProcessChannelUpdateFromRssAsync(response.Feed, cancellationToken);
            var processEntriesTask = _entryService.ProcessEntriesFromRSSAsync(response.Feed, cancellationToken);
            var updateFeedTask = UpdateFeedFromResponseAsync(feed, response, cancellationToken);

            await Task.WhenAll(processChannelTask, processEntriesTask, updateFeedTask);
        }

        private async Task UpdateFeedFromResponseAsync(Feed feed, ReadRssResponseMessage response, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feed);
            ArgumentNullException.ThrowIfNull(response);

            feed.LastETag = response.ETag ?? feed.LastETag;
            feed.LastModifiedDate = response.ResponseDate ?? feed.LastModifiedDate;
            await _feedRepository.UpdateAsync(feed.FeedId, feed, cancellationToken);
        }
    }
}
