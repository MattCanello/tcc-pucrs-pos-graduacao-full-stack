namespace MattCanello.NewsFeed.Frontend.Server.Models.Search
{
    [Serializable]
    public sealed record SearchDocument
    {
        public string? Id { get; init; }
        public string FeedId { get; init; }
        public SearchEntry Entry { get; init; }
    }
}
