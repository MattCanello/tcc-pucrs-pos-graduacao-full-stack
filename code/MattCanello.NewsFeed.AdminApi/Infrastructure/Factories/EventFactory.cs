using Dapr.Client;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.Dapr.Interfaces;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Factories
{
    public sealed class EventFactory : IEventFactory
    {
        private readonly IBindingRequestFactory _bindingRequestFactory;

        public EventFactory(IBindingRequestFactory bindingRequestFactory)
        {
            _bindingRequestFactory = bindingRequestFactory;
        }

        public BindingRequest CreateNewFeedCreatedEvent(Feed feed, string bindingName)
        {
            ArgumentNullException.ThrowIfNull(feed);
            ArgumentNullException.ThrowIfNull(bindingName);

            var bindingRequest = _bindingRequestFactory
                .CreateBindingRequest(feed, bindingName);

            bindingRequest
                .SetFeedId(feed.FeedId);

            return bindingRequest;
        }
    }
}
