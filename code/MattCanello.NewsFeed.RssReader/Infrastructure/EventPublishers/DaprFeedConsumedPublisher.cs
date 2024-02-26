using Dapr.Client;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.EventPublishers
{
    public sealed class DaprFeedConsumedPublisher : IFeedConsumedPublisher
    {
        const string BindingName = "rsschannelstopic";
        private readonly IEventFactory _eventFactory;
        private readonly DaprClient _daprClient;

        public DaprFeedConsumedPublisher(DaprClient daprClient, IEventFactory eventFactory)
        {
            _daprClient = daprClient;
            _eventFactory = eventFactory;
        }

        public async Task PublishAsync(string feedId, DateTimeOffset consumedDate, Channel channel, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(channel);

            var bindingRequest = _eventFactory.CreateFeedConsumedEvent(feedId, consumedDate, channel, BindingName);

            await _daprClient.InvokeBindingAsync(bindingRequest, cancellationToken);
        }
    }
}
