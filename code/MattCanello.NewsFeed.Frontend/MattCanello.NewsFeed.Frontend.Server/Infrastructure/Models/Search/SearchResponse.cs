namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search
{
    [Serializable]
    public sealed record SearchResponse<TModel> where TModel : class, new()
    {
        public long Total { get; set; }

        public IReadOnlyList<TModel> Results { get; set; }
            = Array.Empty<TModel>();

        public SearchPaging? Paging { get; init; }

        public static SearchResponse<TModel> CreateEmpty(int? pageSize, int? skip)
        {
            return new SearchResponse<TModel>()
            {
                Paging = new SearchPaging(pageSize, skip)
            };
        }

        public static SearchResponse<TModel> CreateEmpty(SearchCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            return CreateEmpty(command.PageSize, command.Skip);
        }
    }
}
