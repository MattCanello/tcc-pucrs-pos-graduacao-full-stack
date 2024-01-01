using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class EntryPublisher : IEntryPublisher
    {
        public Task PublishNewEntryAsync(Entry entry, CancellationToken cancellationToken = default)
        {
            // TODO: Implementar
            return Task.CompletedTask;
        }
    }
}
