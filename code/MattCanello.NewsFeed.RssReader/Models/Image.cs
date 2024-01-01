using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Models
{
    [Serializable]
    public sealed class Image
    {
        [DataType(DataType.ImageUrl)]
        public string? Url { get; set; }
    }
}
