using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Repositories
{
    public sealed class AdminFeedRepository : IFeedRepository
    {
        private readonly IAdminClient _adminClient;
        private readonly IMapper _mapper;

        public AdminFeedRepository(IAdminClient adminClient, IMapper mapper)
        {
            _adminClient = adminClient;
            _mapper = mapper;
        }

        public async Task<(Feed, Channel)> GetFeedAndChannelAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var adminFeed = await _adminClient.GetFeedAsync(feedId, cancellationToken);

            var feed = _mapper.Map<Feed>(adminFeed);

            var channel = _mapper.Map<Channel>(adminFeed.Channel);

            return (feed, channel);
        }

        public async Task<Feed> GetFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var adminFeed = await _adminClient.GetFeedAsync(feedId, cancellationToken);

            var feed = _mapper.Map<Feed>(adminFeed);

            return feed;
        }
    }
}
