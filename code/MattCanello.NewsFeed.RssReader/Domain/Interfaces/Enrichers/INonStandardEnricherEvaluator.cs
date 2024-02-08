namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Enrichers
{
    public interface INonStandardEnricherEvaluator
    {
        INonStandardRssEnricher? Evaluate(string @namespace);
    }
}
