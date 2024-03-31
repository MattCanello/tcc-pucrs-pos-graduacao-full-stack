using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Domain.Commands
{
    [Serializable]
    public sealed class CreateFeedCommand
    {
        [Required]
        [StringLength(100)]
        public string? FeedId { get; set; }

        [Required]
        [StringLength(100)]
        public string? ChannelId { get; set; }

        [Url]
        [Required]
        [DataType(DataType.Url)]
        public string? Url { get; set; }
    }
}
