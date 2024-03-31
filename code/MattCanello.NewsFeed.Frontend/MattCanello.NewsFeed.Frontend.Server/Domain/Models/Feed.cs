using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed record Feed
    {
        public Feed() { }

        public Feed(string feedId, string? name = null)
        {
            FeedId = feedId;
            Name = name;
        }

        [Required]
        public string? FeedId { get; set; }

        public string? Name { get; set; }
    }
}
