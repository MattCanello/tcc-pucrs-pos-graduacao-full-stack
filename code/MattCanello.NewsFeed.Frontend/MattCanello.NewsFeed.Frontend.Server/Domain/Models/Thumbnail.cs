using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed record Thumbnail
    {
        public Thumbnail() { }

        public Thumbnail(string imageUrl, string? credit = null, string? caption = null)
        {
            ImageUrl = imageUrl;
            Credit = credit;
            Caption = caption;
        }

        [Url]
        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }
        public string? Credit { get; set; }
        public string? Caption { get; set; }
    }
}
