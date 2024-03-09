namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Evalulators
{
    public interface IMostRecentPublishDateEvaluator
    {
        DateTimeOffset Evaluate(DateTimeOffset? current, DateTimeOffset other);
    }
}
