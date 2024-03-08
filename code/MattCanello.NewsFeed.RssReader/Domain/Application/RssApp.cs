using AutoMapper;
using MattCanello.NewsFeed.RssReader.Domain.Exceptions;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Clients;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Repositories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services;
using MattCanello.NewsFeed.RssReader.Domain.Messages;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Responses;

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

        public async Task<ProcessRssResponse> ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var feed = await _feedRepository.GetAsync(feedId, cancellationToken)
                ?? throw new FeedNotFoundException(feedId);

            var requestMessage = _mapper
                .Map<ReadRssRequestMessage>(feed);

            var readResponse = await _rssClient
                .ReadAsync(requestMessage, cancellationToken);

            if (readResponse.IsNotModified || readResponse.Feed is null)
                return ProcessRssResponse.NotModified;

            var responseDate = readResponse.ResponseDate ?? DateTimeOffset.UtcNow;

            var publishedEntriesResponse = await _entryService.ProcessEntriesFromRSSAsync(feed, readResponse.Feed, cancellationToken);

            var feedConsumedTask = _channelService.ProcessFeedConsumedAsync(feedId, responseDate, readResponse.Feed, cancellationToken);
            var updateFeedTask = UpdateFeedFromResponseAsync(feed, readResponse, publishedEntriesResponse, cancellationToken);

            await Task.WhenAll(feedConsumedTask, updateFeedTask);
            return new ProcessRssResponse(publishedEntriesResponse.PublishedCount);
        }

        private async Task UpdateFeedFromResponseAsync(Feed feed, ReadRssResponseMessage readResponse, PublishRssEntriesResponse publishResponse, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feed);
            ArgumentNullException.ThrowIfNull(readResponse);
            ArgumentNullException.ThrowIfNull(publishResponse);

            feed.SetAsModified(readResponse.ETag, readResponse.ResponseDate, publishResponse.MostRecentPublishDate);

            await _feedRepository.UpdateAsync(feed.FeedId, feed, cancellationToken);
        }
    }
}
