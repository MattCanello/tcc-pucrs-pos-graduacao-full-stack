using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Domain.Responses
{
    [Serializable]
    public sealed record ParsedContent
    {
        public ParsedContent(string? description = null, Thumbnail? thumb = null)
        {
            Description = description;
            Thumbnail = thumb;
        }

        [DataType(DataType.Html)]
        public string? Description { get; init; }
        public Thumbnail? Thumbnail { get; init; }
    }
}
