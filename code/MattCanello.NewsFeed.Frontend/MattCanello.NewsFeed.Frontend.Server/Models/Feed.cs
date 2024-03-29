using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Models
{
    [Serializable]
    public sealed record Feed
    {
        public Feed() { }

        public Feed(string feedId, string? feedName = null)
        {
            FeedId = feedId;
            FeedName = feedName;
        }

        [Required]
        public string? FeedId { get; set; }

        public string? FeedName { get; set; }
    }
}
