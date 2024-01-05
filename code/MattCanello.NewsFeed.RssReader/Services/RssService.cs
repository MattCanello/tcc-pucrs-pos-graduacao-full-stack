using AutoMapper;
using MattCanello.NewsFeed.RssReader.Exceptions;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Messages;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class RssService : IRssService
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IMapper _mapper;
        private readonly IRssClient _rssClient;

        private readonly IChannelService _channelService;
        private readonly IEntryService _entryService;

        public RssService(IFeedRepository feedRepository, IMapper mapper, IRssClient rssClient, IChannelService channelService, IEntryService entryService)
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

            var processChannelTask = _channelService.ProcessChannelUpdateFromRssAsync(feed.ChannelId, response.Feed, cancellationToken);
            var processEntriesTask = _entryService.ProcessEntriesFromRSSAsync(feedId, response.Feed, cancellationToken);

            await Task.WhenAll(processChannelTask, processEntriesTask);
            await UpdateFeedFromResponseAsync(feed, response, cancellationToken);
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
