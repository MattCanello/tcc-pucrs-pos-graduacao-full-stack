using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IAdminClient
    {
        Task<AdminQueryResponse<AdminChannel>> QueryChannelsAsync(AdminQueryCommand queryCommand, CancellationToken cancellationToken = default);
        Task<AdminFeedWithChannel> GetFeedAsync(string feedId, CancellationToken cancellationToken = default);
    }
}
