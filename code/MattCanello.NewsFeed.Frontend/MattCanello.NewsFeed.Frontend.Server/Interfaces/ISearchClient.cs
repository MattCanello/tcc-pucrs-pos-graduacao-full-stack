using MattCanello.NewsFeed.Frontend.Server.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Interfaces
{
    public interface ISearchClient
    {
        Task<SearchRecentResponse> GetRecentAsync(string? feedId = null, int? size = null, CancellationToken cancellationToken = default);
        Task<SearchResponse<SearchDocument>> SearchAsync(SearchCommand command, CancellationToken cancellationToken = default);
    }
}
