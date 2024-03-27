using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Services
{
    public sealed class FeedService : IFeedService
    {
        private readonly IFeedRepository _feedRepository;

        public FeedService(IFeedRepository feedRepository) 
            => _feedRepository = feedRepository;

        public async Task<Feed> UpdateFeedAsync(string feedId, ChannelData channelData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(channelData);

            var feed = await _feedRepository.GetByIdAsync(feedId, cancellationToken)
                ?? throw new FeedNotFoundException(feedId);

            // TODO: Transformar em mapping com ValueResolver
            feed.Copyright ??= channelData.Copyright;
            feed.ImageUrl ??= channelData.ImageUrl;
            feed.Language ??= channelData.Language;
            feed.Name ??= channelData.Name;

            feed = await _feedRepository.UpdateAsync(feed, cancellationToken);

            return feed;
        }
    }
}
