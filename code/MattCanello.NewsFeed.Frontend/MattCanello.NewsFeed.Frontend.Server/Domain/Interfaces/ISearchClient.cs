using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface ISearchClient
    {
        Task<SearchResponse<SearchDocument>> GetRecentAsync(string? channelId = null, int? size = null, CancellationToken cancellationToken = default);
        Task<SearchResponse<SearchDocument>> SearchAsync(SearchCommand command, CancellationToken cancellationToken = default);
        Task<SearchDocument?> GetDocumentByIdAsync(string feedId, string id, CancellationToken cancellationToken = default);
    }
}
