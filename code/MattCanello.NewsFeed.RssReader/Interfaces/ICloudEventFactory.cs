using CloudNative.CloudEvents;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface ICloudEventFactory
    {
        CloudEvent CreateNewEntryEvent(string feedId, Entry entry);
        CloudEvent CreateChannelUpdatedEvent(string channelId, Channel channel);
    }
}
