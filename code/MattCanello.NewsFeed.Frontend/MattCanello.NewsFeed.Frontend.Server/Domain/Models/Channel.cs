namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed record Channel
    {
        public string? ChannelId { get; set; }

        public string? Name { get; set; }
    }
}
