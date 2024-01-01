using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IEntryPublisher
    {
        Task PublishNewEntryAsync(Entry entry);
    }
}
