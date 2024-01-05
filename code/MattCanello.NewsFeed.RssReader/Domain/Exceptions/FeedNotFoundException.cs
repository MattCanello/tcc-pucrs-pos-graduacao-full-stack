namespace MattCanello.NewsFeed.RssReader.Domain.Exceptions
{
    public sealed class FeedNotFoundException : ApplicationException
    {
        public FeedNotFoundException()
            : base("The provided feedId was not found.") { }

        public FeedNotFoundException(string feedId)
            : base($"The provided feedId was not found. The provided feedId was '{feedId}'.")
        {
            FeedId = feedId;
        }

        public string? FeedId { get; }
    }
}
