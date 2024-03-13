using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Domain.Exceptions
{
    [Serializable]
    public sealed class EntryAlreadyIndexedException : IndexException
    {
        private const string DefaultMessage = "The provided entry was already indexed previously.";

        public EntryAlreadyIndexedException()
            : base(DefaultMessage, null) { }

        public EntryAlreadyIndexedException(string indexName)
            : base(indexName, DefaultMessage, null) { }

        public EntryAlreadyIndexedException(Document document) : this()
            => Document = document;

        public EntryAlreadyIndexedException(Document document, string indexName) : this(indexName)
            => Document = document;

        public Document? Document { get; }
    }
}
