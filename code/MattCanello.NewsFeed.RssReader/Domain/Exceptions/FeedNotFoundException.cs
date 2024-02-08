namespace MattCanello.NewsFeed.RssReader.Domain.Exceptions
{
    public sealed class FeedNotFoundException : ApplicationException
    {
        const string BaseMessage = "The provided feedId was not found.";

        public FeedNotFoundException()
            : base(BaseMessage) { }

        public FeedNotFoundException(string feedId)
            : base($"{BaseMessage} The provided feedId was '{feedId}'.")
        {
            FeedId = feedId;
        }

        public string? FeedId { get; }
    }
}
