namespace MattCanello.NewsFeed.Frontend.Server.Models
{
    [Serializable]
    public sealed record Channel
    {
        public Channel() { }

        public Channel(string channelId, string channelName)
        {
            ChannelId = channelId;
            ChannelName = channelName;
        }

        public string? ChannelId { get; set; }

        public string? ChannelName { get; set; }
    }
}
