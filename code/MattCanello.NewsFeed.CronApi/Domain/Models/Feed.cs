using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.CronApi.Domain.Models
{
    [Serializable]
    public sealed class Feed
    {
        [Required]
        [StringLength(100)]
        public string FeedId { get; set; }

        public DateTimeOffset? LastExecutionDate { get; set; }
    }
}
