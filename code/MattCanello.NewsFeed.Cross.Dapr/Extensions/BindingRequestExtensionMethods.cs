using Dapr.Client;

namespace MattCanello.NewsFeed.Cross.Dapr.Extensions
{
    public static class BindingRequestExtensionMethods
    {
        public static void SetFeedId(this BindingRequest bindingRequest, string feedId)
        {
            ArgumentNullException.ThrowIfNull(bindingRequest);
            ArgumentNullException.ThrowIfNull(feedId);

            bindingRequest.Metadata["partitionKey"] = feedId;
            bindingRequest.Metadata["PartitionKey"] = feedId;
        }
    }
}
