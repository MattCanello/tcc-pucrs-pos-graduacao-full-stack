using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Services
{
    public sealed class FeedService : IFeedService
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IMapper _mapper;

        public FeedService(IFeedRepository feedRepository, IMapper mapper)
        {
            _feedRepository = feedRepository;
            _mapper = mapper;
        }

        public async Task<Feed> UpdateFeedAsync(string feedId, RssData data, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(data);

            var feed = await _feedRepository.GetByIdAsync(feedId, cancellationToken)
                ?? throw new FeedNotFoundException(feedId);

            feed = _mapper.Map(data, feed);

            feed = await _feedRepository.UpdateAsync(feed, cancellationToken);

            return feed;
        }
    }
}
