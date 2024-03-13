using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Exceptions
{
    [Serializable]
    public sealed class IndexPolicyException : IndexException
    {
        public IndexPolicyException(string indexName, string message, Exception? innerException = null) 
            : base(indexName, message, innerException) { }
    }
}
