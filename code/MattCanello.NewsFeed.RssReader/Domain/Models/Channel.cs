using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Domain.Models
{
    [Serializable]
    public sealed class Channel
    {
        public string? Name { get; set; }

        [DataType(DataType.Url)]
        public string? Url { get; set; }
        public string? Description { get; set; }
        public string? Language { get; set; }
        public string? Copyright { get; set; }
        public Image? Image { get; set; }
    }
}
