using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IChannelApp
    {
        Task<SearchResponse<Document>> GetDocumentsAsync(GetChannelDocumentsCommand command, CancellationToken cancellationToken = default);
    }
}
