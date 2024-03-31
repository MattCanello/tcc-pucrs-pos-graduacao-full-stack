namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search
{
    [Serializable]
    public sealed record SearchEntry
    {
        public string? Id { get; init; }

        public string? Title { get; init; }

        public string? Url { get; init; }

        public string? Description { get; init; }

        public ISet<SearchCategory> Categories { get; init; } = new HashSet<SearchCategory>(capacity: 0);

        public DateTimeOffset PublishDate { get; init; }

        public IList<SearchThumbnail> Thumbnails { get; init; } = Array.Empty<SearchThumbnail>();

        public IList<SearchAuthor> Authors { get; init; } = Array.Empty<SearchAuthor>();
    }
}
