using CloudNative.CloudEvents;
using Dapr.Client;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class DaprEntryPublisher : IEntryPublisher
    {
        private readonly DaprClient _daprClient;
        private readonly ICloudEventFactory _cloudEventFactory;
        private readonly CloudEventFormatter _cloudEventFormatter;
        const string BindingName = "rssentriestopic";

        public DaprEntryPublisher(DaprClient daprClient, ICloudEventFactory cloudEventFactory, CloudEventFormatter cloudEventFormatter)
        {
            _daprClient = daprClient;
            _cloudEventFactory = cloudEventFactory;
            _cloudEventFormatter = cloudEventFormatter;
        }

        public async Task PublishNewEntryAsync(string feedId, Entry entry, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(entry);

            var cloudEvent = _cloudEventFactory.CreateNewEntryEvent(feedId, entry);

            var cloudEventData = _cloudEventFormatter.EncodeStructuredModeMessage(cloudEvent, out _);

            var bindingRequest = CreateBindingRequest(feedId, cloudEventData);

            await _daprClient.InvokeBindingAsync(bindingRequest, cancellationToken);
        }

        public static BindingRequest CreateBindingRequest(string feedId, ReadOnlyMemory<byte> data)
        {
            var bindingRequest = new BindingRequest(BindingName, operation: "create")
            {
                Data = data
            };

            bindingRequest.Metadata["partitionKey"] = feedId;
            bindingRequest.Metadata["PartitionKey"] = feedId;

            return bindingRequest;
        }
    }
}
