using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IEntryRepository
    {
        Task<Entry> GetByIdAsync(string feedId, string id, CancellationToken cancellationToken = default);
    }
}
