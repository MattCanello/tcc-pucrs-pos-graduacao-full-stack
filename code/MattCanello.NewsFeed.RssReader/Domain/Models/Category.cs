using System.Runtime.Serialization;

namespace MattCanello.NewsFeed.RssReader.Domain.Models
{
    [Serializable]
    public record Category(string CategoryName, string? Label = null, string? Schema = null);
}
