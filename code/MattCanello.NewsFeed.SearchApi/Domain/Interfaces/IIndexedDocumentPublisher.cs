using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Domain.Interfaces
{
    public interface IIndexedDocumentPublisher
    {
        Task PublishAsync(Document document, CancellationToken cancellationToken = default);
    }
}
