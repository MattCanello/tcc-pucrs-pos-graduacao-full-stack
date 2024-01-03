using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IEntryPublisher
    {
        Task PublishNewEntryAsync(string feedId, Entry entry, CancellationToken cancellationToken = default);
    }
}
