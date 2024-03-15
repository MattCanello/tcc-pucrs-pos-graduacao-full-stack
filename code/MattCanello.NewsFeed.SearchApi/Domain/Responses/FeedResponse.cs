using MattCanello.NewsFeed.SearchApi.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.SearchApi.Domain.Responses
{
    [Serializable]
    public sealed class FeedResponse
    {
        public static readonly FeedResponse Empty = new FeedResponse();

        public FeedResponse() { }

        public FeedResponse(IReadOnlyList<Document> results, long total)
        {
            Results = results;
            Total = total;
        }

        [Required]
        [Range(0, long.MaxValue)]
        public long Total { get; private set; }

        [Required]
        public IReadOnlyList<Document>? Results { get; private set; }

        [JsonIgnore]
        public bool IsEmpty => Total == 0;
    }
}
