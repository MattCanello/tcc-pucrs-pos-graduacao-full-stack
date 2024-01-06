using MattCanello.NewsFeed.RssReader.Infrastructure.Evaluators;
using MattCanello.NewsFeed.RssReader.Infrastructure.Formatters.Rss091;
using MattCanello.NewsFeed.RssReader.Infrastructure.Strategies;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Evaluators
{
    public sealed class SyndicationFeedEvaluatorTests
    {
        private static readonly DefaultSyndicationFeedLoader _default = new DefaultSyndicationFeedLoader();
        private static readonly Rss091SyndicationFeedLoader _rss091 = new Rss091SyndicationFeedLoader(new Rss091Formatter());

        private static XmlReaderSettings ReaderSettings => new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore };

        [Fact]
        public void DetermineLoaderStrategy_ForRss091_ShouldReturnRss091()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_091), ReaderSettings);

            var evaluator = new SyndicationFeedEvaluator(_default, _rss091);

            var strategy = evaluator.DetermineLoaderStrategy(reader);

            Assert.Equal(_rss091, strategy);
        }

        [Fact]
        public void DetermineLoaderStrategy_ForTheGuardian_ShouldReturnDefault()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk), ReaderSettings);

            var evaluator = new SyndicationFeedEvaluator(_default, _rss091);

            var strategy = evaluator.DetermineLoaderStrategy(reader);

            Assert.Equal(_default, strategy);
        }
    }
}
