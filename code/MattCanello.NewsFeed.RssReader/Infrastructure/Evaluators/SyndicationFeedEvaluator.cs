using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Evaluators;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Strategies;
using MattCanello.NewsFeed.RssReader.Infrastructure.Strategies;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Evaluators
{
    public sealed class SyndicationFeedEvaluator : ISyndicationFeedEvaluator
    {
        private readonly DefaultSyndicationFeedLoader _defaultSyndicationFeedLoader;
        private readonly Rss091SyndicationFeedLoader _rss091SyndicationFeedLoader;

        public SyndicationFeedEvaluator(
            DefaultSyndicationFeedLoader defaultSyndicationFeedLoader, 
            Rss091SyndicationFeedLoader rss091SyndicationFeedLoader)
        {
            _defaultSyndicationFeedLoader = defaultSyndicationFeedLoader;
            _rss091SyndicationFeedLoader = rss091SyndicationFeedLoader;
        }

        public ISyndicationFeedLoaderStrategy DetermineLoaderStrategy(XmlReader reader)
        {
            if (_rss091SyndicationFeedLoader.CanRead(reader))
                return _rss091SyndicationFeedLoader;

            return _defaultSyndicationFeedLoader;
        }
    }
}
