namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface INonStandardEnricherEvaluator
    {
        INonStandardRssEnricher? Evaluate(string @namespace);
    }
}
