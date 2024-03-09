using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Evalulators;

namespace MattCanello.NewsFeed.RssReader.Domain.Evaluators
{
    public sealed class MostRecentPublishDateEvaluator : IMostRecentPublishDateEvaluator
    {
        public DateTimeOffset Evaluate(DateTimeOffset? current, DateTimeOffset entryPublishDate)
        {
            if (current is null)
                return entryPublishDate;

            if (current > entryPublishDate)
                return current.Value;

            return entryPublishDate;
        }
    }
}
