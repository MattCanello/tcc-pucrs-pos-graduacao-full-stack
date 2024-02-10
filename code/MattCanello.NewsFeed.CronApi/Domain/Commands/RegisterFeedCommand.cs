using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.CronApi.Domain.Commands
{
    [Serializable]
    public sealed class RegisterFeedCommand
    {
        [Required]
        [StringLength(100)]
        public string? FeedId { get; set; }
    }
}
