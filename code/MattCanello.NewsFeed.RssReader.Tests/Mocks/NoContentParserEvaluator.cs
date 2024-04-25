using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Parsers;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    sealed class NoContentParserEvaluator : IContentParserEvaluator
    {
        public static readonly NoContentParserEvaluator Instance = new NoContentParserEvaluator();

        public IContentParser? EvaluateParser(SyndicationItem item) 
            => null;
    }
}
