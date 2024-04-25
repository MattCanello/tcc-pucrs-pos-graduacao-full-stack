using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Parsers
{
    public interface IContentParserEvaluator
    {
        IContentParser? EvaluateParser(SyndicationItem item);
    }
}
