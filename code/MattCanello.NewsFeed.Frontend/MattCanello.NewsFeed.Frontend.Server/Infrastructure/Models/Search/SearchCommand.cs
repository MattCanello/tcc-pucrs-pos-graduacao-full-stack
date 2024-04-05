using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search
{
    [Serializable]
    public sealed record SearchCommand
    {
        [JsonPropertyName("q")]
        public string? Query { get; init; }

        [JsonPropertyName("size")]
        public int? PageSize { get; init; }

        [JsonPropertyName("skip")]
        public int? Skip { get; init; }

        [JsonPropertyName("feedId")]
        public string? FeedId { get; init; }
    }
}
