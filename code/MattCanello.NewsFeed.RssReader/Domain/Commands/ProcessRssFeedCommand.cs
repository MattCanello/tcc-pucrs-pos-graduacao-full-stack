using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Domain.Commands
{
    [Serializable]
    public sealed class ProcessRssFeedCommand
    {
        [Required]
        [StringLength(100)]
        public string? FeedId { get; set; }
    }
}
