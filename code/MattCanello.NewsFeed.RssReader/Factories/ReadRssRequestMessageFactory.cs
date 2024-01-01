using MattCanello.NewsFeed.RssReader.Messages;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Factories
{
    public sealed class ReadRssRequestMessageFactory
    {
        public ReadRssRequestMessage FromFeed(Feed feed)
        {
            ArgumentNullException.ThrowIfNull(feed);

            return new ReadRssRequestMessage(new Uri(feed.Url, UriKind.Absolute), feed.LastETag, feed.LastModifiedDate);
        }
    }
}
