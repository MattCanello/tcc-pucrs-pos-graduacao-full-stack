using Dapr.Client;
using MattCanello.NewsFeed.Cross.Dapr.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Factories
{
    public sealed class EventFactory : IEventFactory
    {
        private readonly IBindingRequestFactory _bindingRequestFactory;

        public EventFactory(IBindingRequestFactory bindingRequestFactory)
        {
            _bindingRequestFactory = bindingRequestFactory;
        }

        public BindingRequest? CreateIndexedDocumentEvent(Document document, string bindingName)
        {
            ArgumentNullException.ThrowIfNull(document);
            ArgumentNullException.ThrowIfNull(bindingName);

            var bindingRequest = _bindingRequestFactory
                .CreateBindingRequest(new { documentId = document.Id, document.FeedId, entryId = document.Entry?.Id }, bindingName);

            bindingRequest
                .SetFeedId(document.FeedId);

            return bindingRequest;
        }
    }
}
