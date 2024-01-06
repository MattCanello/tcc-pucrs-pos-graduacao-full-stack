using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Strategies;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Evaluators
{
    public interface ISyndicationFeedEvaluator
    {
        ISyndicationFeedLoaderStrategy DetermineLoaderStrategy(XmlReader reader);
    }
}
