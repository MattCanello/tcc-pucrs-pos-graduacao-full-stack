using Dapr.Client;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class DaprEntryPublisher : IEntryPublisher
    {
        private readonly DaprClient _daprClient;
        const string BindingName = "entriestopic";

        public DaprEntryPublisher(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task PublishNewEntryAsync(string feedId, Entry entry, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(entry);

            var metadata = new Dictionary<string, string>(capacity: 2) 
            {
                { "partitionKey", feedId },
                { "PartitionKey", feedId }
            };

            await _daprClient.InvokeBindingAsync(BindingName, operation: "create", entry, metadata, cancellationToken);
        }
    }
}
