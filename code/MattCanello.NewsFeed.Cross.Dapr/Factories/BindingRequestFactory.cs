using Dapr.Client;
using MattCanello.NewsFeed.Cross.Dapr.Interfaces;
using System.Text.Json;

namespace MattCanello.NewsFeed.Cross.Dapr.Factories
{
    public sealed class BindingRequestFactory : IBindingRequestFactory
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public BindingRequestFactory(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public BindingRequest CreateBindingRequest(object data, string bindingName, string operation = "create")
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(bindingName);
            ArgumentNullException.ThrowIfNull(operation);

            using var dataStream = new MemoryStream();

            using var writer = new Utf8JsonWriter(dataStream);

            JsonSerializer.Serialize(writer, data, _jsonSerializerOptions);

            dataStream.Position = 0;

            ReadOnlyMemory<byte> binaryData = dataStream.ToArray();

            var bindingRequest = new BindingRequest(bindingName, operation)
            {
                Data = binaryData
            };

            return bindingRequest;
        }
    }
}
