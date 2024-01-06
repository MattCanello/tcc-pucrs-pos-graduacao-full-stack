using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Strategies;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Strategies
{
    public sealed class DefaultSyndicationFeedLoader : ISyndicationFeedLoaderStrategy
    {
        public bool CanRead(XmlReader reader)
        {
            return true;
        }

        public SyndicationFeed Load(XmlReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            return SyndicationFeed.Load(reader);
        }
    }
}
