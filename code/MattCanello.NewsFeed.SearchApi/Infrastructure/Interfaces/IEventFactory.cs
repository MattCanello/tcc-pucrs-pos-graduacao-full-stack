using Dapr.Client;
using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Interfaces
{
    public interface IEventFactory
    {
        BindingRequest? CreateIndexedDocumentEvent(Document document, string bindingName);
    }
}
