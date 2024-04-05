namespace MattCanello.NewsFeed.Frontend.Server.Domain.Exceptions
{
    [Serializable]
    public sealed class FeedNotFoundException : Exception
    {
        const string DefaultMessage = "The requested feed was not found";

        public FeedNotFoundException() 
            : base(DefaultMessage) { }

        public FeedNotFoundException(string feedId, string? message = null)
            : base(message ?? DefaultMessage) => FeedId = feedId;

        public string? FeedId { get; }
    }
}
