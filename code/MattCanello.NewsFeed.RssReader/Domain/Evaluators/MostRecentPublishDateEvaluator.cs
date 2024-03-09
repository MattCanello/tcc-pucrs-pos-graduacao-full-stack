using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Evalulators;

namespace MattCanello.NewsFeed.RssReader.Domain.Evaluators
{
    public sealed class MostRecentPublishDateEvaluator : IMostRecentPublishDateEvaluator
    {
        public DateTimeOffset Evaluate(DateTimeOffset? current, DateTimeOffset other)
        {
            if (current is null)
                return other;

            if (current > other)
                return current.Value;

            return other;
        }
    }
}
