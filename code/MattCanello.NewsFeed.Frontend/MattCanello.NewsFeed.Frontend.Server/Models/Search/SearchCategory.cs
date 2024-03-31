namespace MattCanello.NewsFeed.Frontend.Server.Models.Search
{
    [Serializable]
    public sealed record SearchCategory
    {
        public string CategoryName { get; set; } = string.Empty;
    }
}
