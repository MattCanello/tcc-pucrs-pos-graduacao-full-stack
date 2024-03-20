using Dapr.Client;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.EventPublishers
{
    public sealed class DaprIndexedDocumentPublisher : IIndexedDocumentPublisher
    {
        private readonly DaprClient _daprClient;
        private readonly IEventFactory _eventFactory;
        const string BindingName = "indexeddocument";

        public DaprIndexedDocumentPublisher(DaprClient daprClient, IEventFactory eventFactory)
        {
            _daprClient = daprClient;
            _eventFactory = eventFactory;
        }

        public async Task PublishAsync(Document document, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(document);

            var bindingRequest = _eventFactory.CreateIndexedDocumentEvent(document, BindingName);

            await _daprClient.InvokeBindingAsync(bindingRequest, cancellationToken);
        }
    }
}
