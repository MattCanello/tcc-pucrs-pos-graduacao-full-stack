namespace MattCanello.NewsFeed.AdminApi.Domain.Exceptions
{
    public sealed class ChannelNotFoundException : ApplicationException
    {
        private const string DefaultMessage = "The requested channel was not found";

        public ChannelNotFoundException(string channelId) : base(DefaultMessage) 
            => ChannelId = channelId;

        public string ChannelId { get; }
    }
}
