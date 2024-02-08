using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Enrichers;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    internal sealed class SingleNonStandardEnricherEvaluator : INonStandardEnricherEvaluator
    {
        private readonly string _namespace;
        private readonly INonStandardRssEnricher _enricher;

        public SingleNonStandardEnricherEvaluator(string @namespace, INonStandardRssEnricher enricher)
        {
            _namespace = @namespace;
            _enricher = enricher;
        }

        public INonStandardRssEnricher? Evaluate(string @namespace)
        {
            if (_namespace.Equals(@namespace, StringComparison.OrdinalIgnoreCase))
                return _enricher;

            return null;
        }
    }
}
