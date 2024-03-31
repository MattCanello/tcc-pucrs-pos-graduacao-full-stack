using Dapr.Client;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.EventPublishers
{
    public sealed class DaprNewEntryFoundPublisher : INewEntryFoundPublisher
    {
        private readonly DaprClient _daprClient;
        private readonly IEventFactory _eventFactory;
        const string BindingName = "rssentriestopic";

        public DaprNewEntryFoundPublisher(DaprClient daprClient, IEventFactory eventFactory)
        {
            _daprClient = daprClient;
            _eventFactory = eventFactory;
        }

        public async Task PublishAsync(string feedId, string channelId, Entry entry, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(entry);

            var bindingRequest = _eventFactory.CreateNewEntryFoundEvent(feedId, channelId, entry, BindingName);

            await _daprClient.InvokeBindingAsync(bindingRequest, cancellationToken);
        }
    }
}
