using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Repositories
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
    }
}
