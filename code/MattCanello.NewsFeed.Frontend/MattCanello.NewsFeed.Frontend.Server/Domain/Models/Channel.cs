namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed record Channel
    {
        public Channel() { }

        public Channel(string channelId, string name)
        {
            ChannelId = channelId;
            Name = name;
        }

        public string? ChannelId { get; set; }

        public string? Name { get; set; }
    }
}
