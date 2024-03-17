using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public sealed record Document
    {
        public Document(string id, string feedId, Entry entry)
        {
            Id = id;
            FeedId = feedId;
            Entry = entry;
        }

        [Required]
        public string Id { get; init; }

        [Required]
        public string FeedId { get; init; }

        [Required]
        public Entry Entry { get; init; }
    }
}
