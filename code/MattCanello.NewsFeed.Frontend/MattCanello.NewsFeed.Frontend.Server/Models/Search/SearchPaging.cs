namespace MattCanello.NewsFeed.Frontend.Server.Models.Search
{
    [Serializable]
    public sealed record SearchPaging
    {
        public int? Skip { get; init; }
        public int? Size { get; init; }
    }
}
