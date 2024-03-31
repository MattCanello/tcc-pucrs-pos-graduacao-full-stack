namespace MattCanello.NewsFeed.Frontend.Server.Models.Search
{
    [Serializable]
    public sealed record SearchAuthor
    {
        public string Name { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? URL { get; set; }
    }
}
