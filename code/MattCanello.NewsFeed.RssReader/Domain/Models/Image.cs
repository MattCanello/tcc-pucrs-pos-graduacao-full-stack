using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Domain.Models
{
    [Serializable]
    public sealed class Image
    {
        [DataType(DataType.ImageUrl)]
        public string? Url { get; set; }
    }
}
