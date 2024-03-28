namespace MattCanello.NewsFeed.AdminApi.Domain.Exceptions
{
    public sealed class ChannelAlreadyExistsException : ApplicationException
    {
        private const string DefaultMessage = "The channel already exists";

        public ChannelAlreadyExistsException(string channelId) : base(DefaultMessage) 
            => ChannelId = channelId;

        public string ChannelId { get; }
    }
}
