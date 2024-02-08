using MattCanello.NewsFeed.RssReader.Infrastructure.Strategies;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Strategies
{
    public sealed class DefaultSyndicationFeedLoaderTests
    {
        private static XmlReaderSettings ReaderSettings => new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore };

        [Fact]
        public void CanRead_Rss091Sample_ShouldReturnTrue()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_091), ReaderSettings);

            var loader = new DefaultSyndicationFeedLoader();

            var result = loader.CanRead(reader);

            Assert.True(result);
        }

        [Fact]
        public void CanRead_TheGuardianSample_ShouldReturnTrue()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk), ReaderSettings);

            var loader = new DefaultSyndicationFeedLoader();

            var result = loader.CanRead(reader);

            Assert.True(result);
        }

        [Fact]
        public void CanRead_EvenWhenXmlReaderIsNull_ShouldReturnTrue()
        {
            var loader = new DefaultSyndicationFeedLoader();

            var result = loader.CanRead(null!);

            Assert.True(result);
        }

        [Fact]
        public void Load_TheGuardianSample_ShouldReturnFeed()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk), ReaderSettings);

            var loader = new DefaultSyndicationFeedLoader();

            var feed = loader.Load(reader);

            Assert.NotNull(feed);
        }
    }
}
