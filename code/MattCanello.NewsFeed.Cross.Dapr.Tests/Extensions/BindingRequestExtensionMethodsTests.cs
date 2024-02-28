using AutoFixture.Xunit2;
using Dapr.Client;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;

namespace MattCanello.NewsFeed.Cross.Dapr.Tests.Extensions
{
    public sealed class BindingRequestExtensionMethodsTests
    {
        [Theory, AutoData]
        public void SetFeedId_GivenNullBindingRequest_ShouldThrowException(string feedId)
        {
            BindingRequest bindingRequest = null!;

            Assert.Throws<ArgumentNullException>(() => bindingRequest.SetFeedId(feedId));
        }

        [Theory, AutoData]
        public void SetFeedId_GivenNullFeedId_ShouldThrowException(BindingRequest bindingRequest)
        {
            Assert.Throws<ArgumentNullException>(() => bindingRequest.SetFeedId(null!));
        }

        [Theory, AutoData]
        public void SetFeedId_GivenFeedId_ShouldPersistAsPartitionKey(BindingRequest bindingRequest, string feedId)
        {
            bindingRequest.SetFeedId(feedId);

            Assert.Equal(feedId, bindingRequest.Metadata["partitionKey"]);
            Assert.Equal(feedId, bindingRequest.Metadata["PartitionKey"]);
        }
    }
}
