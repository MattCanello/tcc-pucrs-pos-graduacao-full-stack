using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Cross.Dapr.Factories;
using System.Text.Json;

namespace MattCanello.NewsFeed.Cross.Dapr.Tests.Factories
{
    public sealed class BindingRequestFactoryTests
    {
        [Theory, AutoData]
        public void CreateBindingRequest_GivenNullData_ShouldThrowException(string bindingName)
        {
            var factory = new BindingRequestFactory(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            Assert.Throws<ArgumentNullException>(() => factory.CreateBindingRequest(null!, bindingName));
        }

        [Theory, AutoData]
        public void CreateBindingRequest_GivenNullBindingName_ShouldThrowException(object data)
        {
            var factory = new BindingRequestFactory(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            Assert.Throws<ArgumentNullException>(() => factory.CreateBindingRequest(data, null!));
        }

        [Theory, AutoData]
        public void CreateBindingRequest_GivenNullOperation_ShouldThrowException(object data, string bindingName)
        {
            var factory = new BindingRequestFactory(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            Assert.Throws<ArgumentNullException>(() => factory.CreateBindingRequest(data, bindingName, null!));
        }

        [Theory, AutoData]
        public void CreateBindingRequest_GivenExplicitOperation_ShouldRespectGivenOperation(object data, string bindingName, string operation)
        {
            var factory = new BindingRequestFactory(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var bindingRequest = factory.CreateBindingRequest(data, bindingName, operation);

            Assert.Equal(operation, bindingRequest.Operation);
        }

        [Theory, AutoData]
        public void CreateBindingRequest_GivenNoExplicitOperation_ShouldBeCreate(object data, string bindingName)
        {
            var factory = new BindingRequestFactory(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var bindingRequest = factory.CreateBindingRequest(data, bindingName);

            Assert.Equal("create", bindingRequest.Operation);
        }
    }
}
