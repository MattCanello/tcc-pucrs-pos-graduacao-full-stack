using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Enrichers;
using MattCanello.NewsFeed.RssReader.Domain.Services;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Services
{
    public sealed class NonStandardEnricherEvaluatorTests
    {
        [Theory, AutoData]
        public void Evaluate_WhenNamespaceIsUnknown_ShouldReturnNull(string @namesapce)
        {
            var evaluator = new NonStandardEnricherEvaluator();

            var enricher = evaluator.Evaluate(@namesapce);

            Assert.Null(enricher);
        }

        [Theory]
        [InlineData("http://purl.org/dc/elements/1.1/", typeof(PurlEnricher))]
        [InlineData("http://search.yahoo.com/mrss/", typeof(MediaEnricher))]
        public void Evaluate_ForKnownlNamespace_ShouldReturnExpectedEnricher(string @namespace, Type enricherType)
        {
            var evaluator = new NonStandardEnricherEvaluator();

            var enricher = evaluator.Evaluate(@namespace);

            Assert.NotNull(enricher);
            Assert.IsType(enricherType, enricher);
        }
    }
}
