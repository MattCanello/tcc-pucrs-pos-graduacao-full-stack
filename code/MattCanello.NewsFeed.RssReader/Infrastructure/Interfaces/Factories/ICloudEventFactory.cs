using CloudNative.CloudEvents;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories
{
    public interface ICloudEventFactory
    {
        CloudEvent CreateNewEntryFoundEvent(string feedId, Entry entry);
        CloudEvent CreateFeedConsumedEvent(string feedId, Channel channel);
    }
}
