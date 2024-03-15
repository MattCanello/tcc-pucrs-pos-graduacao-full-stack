namespace MattCanello.NewsFeed.SearchApi.Domain.Responses
{
    [Serializable]
    public sealed record FindResponse<TModel>
        where TModel : class
    {
        public static readonly FindResponse<TModel> NotFound = new FindResponse<TModel>();

        private FindResponse()
        {
            Id = string.Empty;
            Model = null;
            IndexName = string.Empty;
        }

        public FindResponse(string id, TModel model, string indexName)
        {
            Id = id;
            Model = model;
            IndexName = indexName;
        }

        public TModel? Model { get; init; }

        public string Id { get; init; }

        public string IndexName { get; init; }

        public bool IsNotFound => Model is null || string.IsNullOrEmpty(Id);
    }
}
