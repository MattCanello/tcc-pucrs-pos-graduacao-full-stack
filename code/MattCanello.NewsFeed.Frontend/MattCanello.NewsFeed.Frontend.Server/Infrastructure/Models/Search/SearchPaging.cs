namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search
{
    [Serializable]
    public sealed record SearchPaging
    {
        public SearchPaging() { }

        public SearchPaging(int? pageSize, int? skip)
        {
            Size = pageSize;
            Skip = skip;
        }

        public int? Skip { get; init; }
        public int? Size { get; init; }
    }
}
