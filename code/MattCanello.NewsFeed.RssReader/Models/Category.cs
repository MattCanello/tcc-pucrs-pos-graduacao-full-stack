using System.Runtime.Serialization;

namespace MattCanello.NewsFeed.RssReader.Models
{
    [Serializable]
    public record Category(string CategoryName, string? Label = null, string? Schema = null);
}
