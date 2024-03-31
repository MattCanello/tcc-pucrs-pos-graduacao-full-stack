namespace MattCanello.NewsFeed.Frontend.Server.Models.Admin
{
    [Serializable]
    public sealed record AdminFeed
    {
        public string FeedId { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string ChannelId { get; set; } = string.Empty;

        public string? Name { get; set; }

        public string? Language { get; set; }

        public string? Copyright { get; set; }

        public string? ImageUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
