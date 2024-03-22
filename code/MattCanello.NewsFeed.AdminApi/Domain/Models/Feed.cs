using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Models
{
    [Serializable]
    public sealed class Feed
    {
        [Required]
        [StringLength(100)]
        public string FeedId { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        public Channel? Channel { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
