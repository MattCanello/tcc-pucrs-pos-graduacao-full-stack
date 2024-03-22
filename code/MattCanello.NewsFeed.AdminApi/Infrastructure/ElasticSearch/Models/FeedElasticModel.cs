namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Models
{
    [Serializable]
    public sealed class FeedElasticModel
    {
        public string? FeedId { get; set; }

        public string? Url { get; set; }

        public string? ChannelId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
