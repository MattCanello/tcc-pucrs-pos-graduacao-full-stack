using Dapr.Client;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using System.Text.Json;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Enqueuers
{
    public sealed class DaprCronFeedEnqueuer : ICronFeedEnqueuer
    {
        private readonly DaprClient _daprClient;
        const string BindingName = "rsspublishcommands";

        public DaprCronFeedEnqueuer(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task EnqueueFeedToProcessAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            using var dataStream = new MemoryStream();

            using var writer = new Utf8JsonWriter(dataStream);

            JsonSerializer.Serialize(writer, new { feedId });

            dataStream.Position = 0;

            ReadOnlyMemory<byte> simpleData = dataStream.ToArray();

            var bindingRequest = CreateBindingRequest(feedId, simpleData);

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
