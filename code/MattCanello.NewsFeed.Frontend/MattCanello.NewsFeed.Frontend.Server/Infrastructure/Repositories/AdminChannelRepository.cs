using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Repositories
{
    public sealed class AdminChannelRepository : IChannelRepository
    {
        private readonly IAdminClient _adminClient;
        private readonly IMapper _mapper;
        private readonly IChannelConfiguration _channelConfiguration;

        public AdminChannelRepository(IAdminClient adminClient, IMapper mapper, IChannelConfiguration channelConfiguration)
        {
            _adminClient = adminClient;
            _mapper = mapper;
            _channelConfiguration = channelConfiguration;
        }

        public async Task<IEnumerable<Channel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            int skip = 0;
            bool hasMorePages;
            List<Channel>? channels = null;
            AdminQueryResponse<AdminChannel> results;
            var bulkCount = _channelConfiguration.ChannelListBulkCount();

            do
            {
                results = await _adminClient.QueryChannelsAsync(new AdminQueryCommand(bulkCount, skip), cancellationToken);

                channels ??= new List<Channel>(capacity: results.Total);

                foreach (var adminChannel in results.Items)
                {
                    var channel = _mapper.Map<Channel>(adminChannel);

                    channels.Add(channel);
                }

                skip = channels.Count;

                hasMorePages = channels.Count < results.Total;
            }
            while (hasMorePages);

            return channels;
        }
    }
}
