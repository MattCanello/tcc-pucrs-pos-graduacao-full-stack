namespace MattCanello.NewsFeed.Frontend.Server.Models.Search
{
    [Serializable]
    public sealed record SearchRecentResponse
    {
        public static readonly SearchRecentResponse Empty = new SearchRecentResponse();

        public long Total { get; init; }

        public IReadOnlyList<SearchDocument> Results { get; init; } = Array.Empty<SearchDocument>();
    }
}
