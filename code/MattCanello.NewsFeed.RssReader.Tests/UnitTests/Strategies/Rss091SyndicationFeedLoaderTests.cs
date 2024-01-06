using MattCanello.NewsFeed.RssReader.Infrastructure.Formatters.Rss091;
using MattCanello.NewsFeed.RssReader.Infrastructure.Strategies;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Strategies
{
    public class Rss091SyndicationFeedLoaderTests
    {
        private static XmlReaderSettings ReaderSettings => new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore };

        [Fact]
        public void CanRead_Rss091Sample_ShouldReturnTrue()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_091), ReaderSettings);

            var loader = new Rss091SyndicationFeedLoader(new Rss091Formatter());

            var canRead = loader.CanRead(reader);

            Assert.True(canRead);
        }

        [Fact]
        public void CanRead_TheGuardianSample_ShouldReturnFalse()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk), ReaderSettings);

            var loader = new Rss091SyndicationFeedLoader(new Rss091Formatter());

            var canRead = loader.CanRead(reader);

            Assert.False(canRead);
        }

        [Fact]
        public void Load_Rss091Sample_ShouldReturnFeed()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_091), ReaderSettings);

            var loader = new Rss091SyndicationFeedLoader(new Rss091Formatter());

            var feed = loader.Load(reader);

            Assert.NotNull(feed);
        }
    }
}
