namespace MattCanello.NewsFeed.Frontend.Server.Models.Admin
{
    public sealed record AdminChannel
    {
        public string ChannelId { get; init; } = string.Empty;

        public string? Name { get; init; }

        public string? Url { get; init; }

        public string? Copyright { get; init; }

        public string? ImageUrl { get; init; }

        public DateTimeOffset CreatedAt { get; init; }
    }
}
