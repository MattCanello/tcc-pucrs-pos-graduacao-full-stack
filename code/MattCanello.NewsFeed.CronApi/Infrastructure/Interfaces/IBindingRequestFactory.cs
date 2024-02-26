using Dapr.Client;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces
{
    public interface IBindingRequestFactory
    {
        BindingRequest CreateFeedEnqueueBindingRequest(string feedId, string bindingName);
    }
}
