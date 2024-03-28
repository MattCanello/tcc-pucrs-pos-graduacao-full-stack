using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Responses;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IFeedRepository
    {
        Task<FeedWithChannel?> GetByIdAsync(string feedId, CancellationToken cancellationToken = default);

        Task<FeedWithChannel> CreateAsync(FeedWithChannel feed, CancellationToken cancellationToken = default);

        Task<FeedWithChannel> UpdateAsync(FeedWithChannel feed, CancellationToken cancellationToken = default);

        Task<QueryResponse<Feed>> QueryAsync(QueryCommand command, CancellationToken cancellationToken = default);
    }
}
