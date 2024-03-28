namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Models
{
    [Serializable]
    public sealed class FeedElasticModel
    {
        public string? FeedId { get; set; }

        public string? ChannelId { get; set; }

        public string? Url { get; set; }

        public string? Name { get; set; }

        public string? Language { get; set; }

        public string? Copyright { get; set; }

        public string? ImageUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
