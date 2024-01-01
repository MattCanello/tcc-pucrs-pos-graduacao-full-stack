using MattCanello.NewsFeed.RssReader.Messages;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IRssClient
    {
        Task<ReadRssResponseMessage> ReadAsync(ReadRssRequestMessage rssReaderRequest, CancellationToken cancellationToken = default);
    }
}
