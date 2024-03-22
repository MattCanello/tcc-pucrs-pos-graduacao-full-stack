namespace MattCanello.NewsFeed.AdminApi.Domain.Exceptions
{
    public sealed class FeedAlreadyExistingException : ApplicationException
    {
        private const string DefaultMessage = "The feed already exists";

        public FeedAlreadyExistingException(string feedId) : base(DefaultMessage) 
            => FeedId = feedId;

        public string FeedId { get; }
    }
}
