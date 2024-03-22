namespace MattCanello.NewsFeed.AdminApi.Domain.Exceptions
{
    public sealed class FeedNotFoundException : ApplicationException
    {
        private const string DefaultMessage = "The requested feed was not found";

        public FeedNotFoundException(string feedId) : base(DefaultMessage)
        {
            FeedId = feedId;
        }

        public string FeedId { get; }
    }
}
