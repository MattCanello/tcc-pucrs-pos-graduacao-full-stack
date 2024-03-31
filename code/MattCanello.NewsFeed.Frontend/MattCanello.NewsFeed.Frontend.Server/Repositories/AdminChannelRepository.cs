using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Models;
using MattCanello.NewsFeed.Frontend.Server.Models.Admin;

namespace MattCanello.NewsFeed.Frontend.Server.Repositories
{
    public sealed class AdminChannelRepository : IChannelRepository
    {
        const int PageSize = 10;

        private readonly IAdminClient _adminClient;
        private readonly IMapper _mapper;

        public AdminChannelRepository(IAdminClient adminClient, IMapper mapper)
        {
            _adminClient = adminClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Channel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            int skip = 0;
            bool hasMorePages = false;
            List<Channel>? channels = null;
            AdminQueryResponse<AdminChannel> results;

            do
            {
                results = await _adminClient.QueryChannelsAsync(new AdminQueryCommand(PageSize, skip), cancellationToken);

                channels ??= new List<Channel>(capacity: results.Total);

                foreach (var result in results.Items)
                {
                    var channel = _mapper.Map<Channel>(result);

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
