using CloudNative.CloudEvents;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories
{
    public interface ICloudEventFactory
    {
        CloudEvent CreateNewEntryEvent(string feedId, Entry entry);
        CloudEvent CreateChannelUpdatedEvent(string feedId, Channel channel);
    }
}
