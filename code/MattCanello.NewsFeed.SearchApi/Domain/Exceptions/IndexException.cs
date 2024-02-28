namespace MattCanello.NewsFeed.SearchApi.Domain.Exceptions
{
    [Serializable]
    public class IndexException : ApplicationException
    {
        const string DefaultMessage = "Unable to index entry";
        public IndexException()
            : base(DefaultMessage) { }

        public IndexException(Exception? innerException)
            : base(DefaultMessage, innerException) { }

        public IndexException(string? message, Exception? innerException = null)
            : base(message ?? DefaultMessage, innerException) { }
    }
}
