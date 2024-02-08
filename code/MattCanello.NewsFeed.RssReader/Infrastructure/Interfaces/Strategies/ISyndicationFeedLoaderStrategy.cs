using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Strategies
{
    public interface ISyndicationFeedLoaderStrategy
    {
        bool CanRead(XmlReader reader);
        SyndicationFeed Load(XmlReader reader);
    }
}
