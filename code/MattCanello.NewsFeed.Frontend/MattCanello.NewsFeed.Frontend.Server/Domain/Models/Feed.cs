using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed record Feed
    {
        [Required]
        public string? FeedId { get; set; }

        public string? Name { get; set; }
    }
}
