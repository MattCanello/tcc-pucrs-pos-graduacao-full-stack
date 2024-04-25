using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Parsers;
using MattCanello.NewsFeed.RssReader.Domain.Responses;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    sealed class MockedContentParser : IContentParser
    {
        public static readonly MockedContentParser Instance = new MockedContentParser();

        public ParsedContent? TryParse(SyndicationContent content) => throw new NotImplementedException();
    }
}
