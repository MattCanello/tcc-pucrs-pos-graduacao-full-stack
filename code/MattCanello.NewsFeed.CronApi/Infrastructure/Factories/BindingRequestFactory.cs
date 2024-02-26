using Dapr.Client;
using MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces;
using System.Text.Json;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Factories
{
    public sealed class BindingRequestFactory : IBindingRequestFactory
    {
        public BindingRequest CreateFeedEnqueueBindingRequest(string feedId, string bindingName)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(bindingName);

            using var dataStream = new MemoryStream();

            using var writer = new Utf8JsonWriter(dataStream);

            JsonSerializer.Serialize(writer, new { feedId });

            dataStream.Position = 0;

            ReadOnlyMemory<byte> data = dataStream.ToArray();

            var bindingRequest = new BindingRequest(bindingName, operation: "create")
            {
                Data = data
            };

            bindingRequest.Metadata["partitionKey"] = feedId;
            bindingRequest.Metadata["PartitionKey"] = feedId;

            return bindingRequest;
        }
    }
}
