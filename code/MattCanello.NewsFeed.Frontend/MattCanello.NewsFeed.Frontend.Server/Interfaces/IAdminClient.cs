using MattCanello.NewsFeed.Frontend.Server.Models.Admin;

namespace MattCanello.NewsFeed.Frontend.Server.Interfaces
{
    public interface IAdminClient
    {
        Task<AdminQueryResponse<AdminChannel>> QueryChannelsAsync(AdminQueryCommand queryCommand, CancellationToken cancellationToken = default);
        Task<AdminFeedWithChannel> GetFeedAsync(string feedId,  CancellationToken cancellationToken = default);
    }
}
