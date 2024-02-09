using CloudNative.CloudEvents;
using Dapr.Client;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Enqueuers
{
    public sealed class DaprCronFeedEnqueuer : ICronFeedEnqueuer
    {
        private readonly DaprClient _daprClient;
        private readonly ICloudEventFactory _cloudEventFactory;
        private readonly CloudEventFormatter _cloudEventFormatter;
        const string BindingName = "rsspublishcommands";

        public DaprCronFeedEnqueuer(DaprClient daprClient, ICloudEventFactory cloudEventFactory, CloudEventFormatter cloudEventFormatter)
        {
            _daprClient = daprClient;
            _cloudEventFactory = cloudEventFactory;
            _cloudEventFormatter = cloudEventFormatter;
        }

        public async Task EnqueueFeedToProcessAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var cloudEvent = _cloudEventFactory.CreateProcessRssEvent(feedId);

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
