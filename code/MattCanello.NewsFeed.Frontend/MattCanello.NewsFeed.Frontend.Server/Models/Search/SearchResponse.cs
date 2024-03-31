namespace MattCanello.NewsFeed.Frontend.Server.Models.Search
{
    [Serializable]
    public sealed record SearchResponse<TModel> where TModel : class, new()
    {
        public long Total { get; set; }

        public IReadOnlyList<TModel>? Results { get; set; }
            = Array.Empty<TModel>();

        public SearchPaging? Paging { get; init; }

        public static SearchResponse<TModel> CreateEmpty(SearchCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            return new SearchResponse<TModel>()
            {
                Paging = new SearchPaging(command.PageSize, command.Skip)
            };
        }
    }
}
