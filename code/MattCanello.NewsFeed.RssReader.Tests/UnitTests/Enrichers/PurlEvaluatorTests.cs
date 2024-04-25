using MattCanello.NewsFeed.RssReader.Domain.Enrichers;
using MattCanello.NewsFeed.RssReader.Domain.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Enrichers;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Enrichers
{
    public sealed class PurlEvaluatorTests
    {
        private readonly INonStandardEnricherEvaluator evaluator = new SingleNonStandardEnricherEvaluator(
            PurlEnricher.PurlXNamespace.NamespaceName, new PurlEnricher());

        [Fact]
        public void FromRSS_UsingTheGuardianUkSample_ShouldReturnExceptedData()
        {
            using var xml = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk));
            var feed = SyndicationFeed.Load(xml);

            var factory = new EntryFactory(evaluator, NoContentParserEvaluator.Instance);
            var entries = factory.FromRSS(feed);

            Assert.NotNull(entries);
            var singleEntry = Assert.Single(entries);

            Assert.NotNull(singleEntry.Authors);
            Assert.NotEmpty(singleEntry.Authors);

            var author = Assert.Single(singleEntry.Authors);
            Assert.Equal("Gavin Blair in Tokyo and agencies", author.Name);
            Assert.Null(author.Email);
            Assert.Null(author.URL);
        }
    }
}
