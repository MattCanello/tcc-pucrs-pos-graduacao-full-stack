using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed record Thumbnail
    {
        [Url]
        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }
        public string? Credit { get; set; }
        public string? Caption { get; set; }
        public int? Width { get; set; }
    }
}
