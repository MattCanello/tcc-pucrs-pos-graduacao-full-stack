using MattCanello.NewsFeed.RssReader.Domain.Messages;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Clients
{
    public interface IRssClient
    {
        Task<ReadRssResponseMessage> ReadAsync(ReadRssRequestMessage rssReaderRequest, CancellationToken cancellationToken = default);
    }
}
