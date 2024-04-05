namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin
{
    [Serializable]
    public sealed record AdminFeedWithChannel
    {
        public string? FeedId { get; init; }

        public string? Url { get; init; }

        public AdminChannel? Channel { get; init; }

        public string? Name { get; init; }

        public string? Language { get; init; }

        public string? Copyright { get; init; }

        public string? ImageUrl { get; init; }

        public DateTimeOffset CreatedAt { get; init; }
    }
}
