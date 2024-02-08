using MattCanello.NewsFeed.RssReader.Domain.Enrichers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Enrichers;

namespace MattCanello.NewsFeed.RssReader.Domain.Services
{
    public sealed class NonStandardEnricherEvaluator : INonStandardEnricherEvaluator
    {
        private readonly IDictionary<string, INonStandardRssEnricher> _enrichers = new Dictionary<string, INonStandardRssEnricher>(StringComparer.OrdinalIgnoreCase)
        {
            { MediaEnricher.MediaXNamespace.NamespaceName, new MediaEnricher() },
            { PurlEnricher.PurlXNamespace.NamespaceName, new PurlEnricher() }
        };

        public INonStandardRssEnricher? Evaluate(string @namespace)
        {
            ArgumentNullException.ThrowIfNull(@namespace);

            if (_enrichers.TryGetValue(@namespace, out INonStandardRssEnricher? enricher))
                return enricher;

            return null;
        }
    }
}
