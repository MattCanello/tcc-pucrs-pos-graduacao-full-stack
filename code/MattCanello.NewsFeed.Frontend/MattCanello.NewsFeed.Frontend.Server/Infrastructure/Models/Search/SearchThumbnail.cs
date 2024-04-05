namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search
{
    [Serializable]
    public sealed record SearchThumbnail
    {
        public string? Url { get; set; }

        public int? Width { get; set; }

        public string? Credit { get; set; }
    }
}
