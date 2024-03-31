namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin
{
    [Serializable]
    public sealed record AdminQueryResponse<TModel>
        where TModel : class, new()
    {
        public static readonly AdminQueryResponse<TModel> Empty = new AdminQueryResponse<TModel>();

        public int Total { get; init; }

        public IReadOnlyCollection<TModel> Items { get; init; } = Array.Empty<TModel>();
    }
}
