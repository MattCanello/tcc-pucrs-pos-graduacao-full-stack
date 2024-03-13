namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Responses
{
    [Serializable]
    public sealed record FindResponse<TElasticModel>
        where TElasticModel : class, new()
    {
        public static readonly FindResponse<TElasticModel> NotFound = new FindResponse<TElasticModel>();

        private FindResponse()
        {
            Id = string.Empty;
            Model = null;
        }

        public FindResponse(string id, TElasticModel model)
        {
            Id = id;
            Model = model;
        }

        public TElasticModel? Model { get; init; }

        public string Id { get; init; }

        public bool IsNotFound => Model is null || string.IsNullOrEmpty(Id);
    }
}
