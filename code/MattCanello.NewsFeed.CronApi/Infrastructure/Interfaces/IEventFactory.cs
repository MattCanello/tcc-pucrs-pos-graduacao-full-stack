using Dapr.Client;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces
{
    public interface IEventFactory
    {
        BindingRequest CreateProcessRssEvent(string feedId, string bindingName);
    }
}
