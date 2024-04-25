using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Parsers;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Services
{
    public sealed class SingleContentParserEvaluator : IContentParserEvaluator
    {
        private readonly IContentParser _contentParser;

        public SingleContentParserEvaluator(IContentParser contentParser) 
            => _contentParser = contentParser;

        public IContentParser? EvaluateParser(SyndicationItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (item.Content is null)
                return null;

            return _contentParser;
        }
    }
}
