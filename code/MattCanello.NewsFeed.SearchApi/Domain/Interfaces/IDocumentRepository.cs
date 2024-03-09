using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetByIdAsync(string feedId, string id, CancellationToken cancellationToken = default);
    }
}
