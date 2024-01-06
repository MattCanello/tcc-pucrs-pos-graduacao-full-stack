using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Evaluators;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Strategies;
using MattCanello.NewsFeed.RssReader.Infrastructure.Strategies;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class SingleSyndicationFeedLoaderEvaluator : ISyndicationFeedEvaluator
    {
        private readonly ISyndicationFeedLoaderStrategy _strategy;

        public static readonly SingleSyndicationFeedLoaderEvaluator Default = new SingleSyndicationFeedLoaderEvaluator(new DefaultSyndicationFeedLoader());

        public SingleSyndicationFeedLoaderEvaluator(ISyndicationFeedLoaderStrategy strategy)
            => _strategy = strategy;

        public ISyndicationFeedLoaderStrategy DetermineLoaderStrategy(XmlReader reader)
            => _strategy;
    }
}
