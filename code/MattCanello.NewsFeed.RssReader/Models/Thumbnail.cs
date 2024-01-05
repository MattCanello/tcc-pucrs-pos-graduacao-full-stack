using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Models
{
    [Serializable]
    public sealed class Thumbnail
    {
        [DataType(DataType.ImageUrl)]
        public string? Url { get; set; }

        [Range(0, int.MaxValue)]
        public int? Width { get; set; }

        public string? Credit { get; set; }
    }
}
