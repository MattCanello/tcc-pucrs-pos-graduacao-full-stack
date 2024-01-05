using MattCanello.NewsFeed.RssReader.Enrichers;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Services;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Enrichers
{
    public sealed class MediaEvaluatorTests
    {
        private readonly INonStandardEnricherEvaluator evaluator = new SingleNonStandardEnricherEvaluator(
            MediaEnricher.MediaXNamespace.NamespaceName, new MediaEnricher());

        [Fact]
        public void FromRSS_UsingTheGuardianUkSample_ShouldReturnExceptedData()
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var feed = SyndicationFeed.Load(xml);

            var reader = new EntryReader(evaluator);
            var entries = reader.FromRSS(feed);

            Assert.NotNull(entries);
            var singleEntry = Assert.Single(entries);

            Assert.NotNull(singleEntry.Thumbnails);
            Assert.NotEmpty(singleEntry.Thumbnails);

            var thumbnail = singleEntry.Thumbnails[0];
            Assert.Equal("https://i.guim.co.uk/img/media/10d3fe792040e29caf0902ec7ac4d592249a5d99/0_140_3645_2187/master/3645.jpg?width=140&quality=85&auto=format&fit=max&s=03dc08c3bf0990310517ffd1f4fcbf41", thumbnail.Url);
            Assert.Equal(140, thumbnail.Width);
            Assert.Equal("Photograph: Yusuke Fukuhara/Yomiuri Shimbun/AFP/Getty Images", thumbnail.Credit);

            thumbnail = singleEntry.Thumbnails[1];
            Assert.Equal("https://i.guim.co.uk/img/media/10d3fe792040e29caf0902ec7ac4d592249a5d99/0_140_3645_2187/master/3645.jpg?width=460&quality=85&auto=format&fit=max&s=379e2f66f700c4f41e1e36501b3c0aa6", thumbnail.Url);
            Assert.Equal(460, thumbnail.Width);
            Assert.Equal("Photograph: Yusuke Fukuhara/Yomiuri Shimbun/AFP/Getty Images", thumbnail.Credit);
        }
    }
}
