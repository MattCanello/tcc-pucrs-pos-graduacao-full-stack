using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Responses;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Parsers
{
    public interface IContentParser
    {
        ParsedContent? TryParse(SyndicationContent content);
    }
}
