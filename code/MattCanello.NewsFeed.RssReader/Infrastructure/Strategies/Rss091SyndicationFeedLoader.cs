using MattCanello.NewsFeed.RssReader.Infrastructure.Formatters.Rss091;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Strategies;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Strategies
{
    public sealed class Rss091SyndicationFeedLoader : ISyndicationFeedLoaderStrategy
    {
        private readonly Rss091Formatter _formatter;

        public Rss091SyndicationFeedLoader(Rss091Formatter formatter)
        {
            _formatter = formatter;
        }

        public bool CanRead(XmlReader reader)
        {
            return _formatter.CanRead(reader);
        }

        public SyndicationFeed Load(XmlReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            _formatter.ReadFrom(reader);

            return _formatter.Feed;
        }
    }
}
