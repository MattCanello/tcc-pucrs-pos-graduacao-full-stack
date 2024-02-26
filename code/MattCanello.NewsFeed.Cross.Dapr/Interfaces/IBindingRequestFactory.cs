using Dapr.Client;

namespace MattCanello.NewsFeed.Cross.Dapr.Interfaces
{
    public interface IBindingRequestFactory
    {
        BindingRequest CreateBindingRequest(object data, string bindingName, string operation = "create");
    }
}
