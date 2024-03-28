using Dapr.Client;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.EventPublishers
{
    public sealed class DaprEventPublisher : IEventPublisher
    {
        private readonly DaprClient _daprClient;
        private readonly IEventFactory _eventFactory;
        const string BindingName = "feedcreated";

        public DaprEventPublisher(DaprClient daprClient, IEventFactory eventFactory)
        {
            _daprClient = daprClient;
            _eventFactory = eventFactory;
        }

        public async Task PublishFeedCreatedEventAsync(FeedWithChannel feed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feed);

            var bindingRequest = _eventFactory.CreateNewFeedCreatedEvent(feed, BindingName);

            await _daprClient.InvokeBindingAsync(bindingRequest, cancellationToken);
        }
    }
}
