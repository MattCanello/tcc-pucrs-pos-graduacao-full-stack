using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface ISearchApp
    {
        Task<SearchResponse<Document>> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default);
    }
}
