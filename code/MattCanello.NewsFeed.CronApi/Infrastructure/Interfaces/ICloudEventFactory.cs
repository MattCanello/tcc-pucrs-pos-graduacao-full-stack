using CloudNative.CloudEvents;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces
{
    public interface ICloudEventFactory
    {
        CloudEvent CreateProcessRssEvent(string feedId);
    }
}