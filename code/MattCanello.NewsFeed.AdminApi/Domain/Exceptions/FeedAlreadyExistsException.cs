namespace MattCanello.NewsFeed.AdminApi.Domain.Exceptions
{
    public sealed class FeedAlreadyExistsException : ApplicationException
    {
        private const string DefaultMessage = "The feed already exists";

        public FeedAlreadyExistsException(string feedId) : base(DefaultMessage) 
            => FeedId = feedId;

        public string FeedId { get; }
    }
}
