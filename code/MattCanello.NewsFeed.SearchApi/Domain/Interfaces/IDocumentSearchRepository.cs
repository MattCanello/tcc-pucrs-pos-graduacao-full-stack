using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IDocumentSearchRepository
    {
        Task<SearchResponse<Document>> SearchAsync(
            string? query = null, Paging? paging = null, string? feedId = null, CancellationToken cancellationToken = default);

        Task<SearchResponse<Document>> SearchByChannelAsync(
            string? query = null, Paging? paging = null, string? channelId = null, CancellationToken cancellationToken = default);

        Task<FindResponse<Document>> FindByIdAsync(string entryId, string feedId, CancellationToken cancellationToken = default);

        Task<SearchResponse<Document>> GetRecentAsync(
            Paging paging, string? feedId = null, CancellationToken cancellationToken = default);
    }
}
