using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IFeedApp
    {
        Task<FeedResponse> GetFeedAsync(GetFeedCommand command, CancellationToken cancellationToken = default);
    }
}
