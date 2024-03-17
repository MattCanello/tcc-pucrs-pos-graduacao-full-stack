namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Exceptions
{
    [Serializable]
    public class ElasticSearchException : ApplicationException
    {
        const string DefaultMessage = "ElasticSearch operation error";

        public ElasticSearchException()
            : base(DefaultMessage) { }

        public ElasticSearchException(Exception? innerException)
            : base(DefaultMessage, innerException) { }

        public ElasticSearchException(string? message, Exception? innerException = null)
            : base(message ?? DefaultMessage, innerException) { }

        public ElasticSearchException(string indexName, string? message = null, Exception? innerException = null)
            : this(message, innerException)
        {
            IndexName = indexName;
        }

        public string? IndexName { get; private set; }
    }
}
