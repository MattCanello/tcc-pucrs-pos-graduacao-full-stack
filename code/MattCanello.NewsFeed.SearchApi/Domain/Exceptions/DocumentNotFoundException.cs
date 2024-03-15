namespace MattCanello.NewsFeed.SearchApi.Domain.Exceptions
{
    [Serializable]
    public class DocumentNotFoundException : ApplicationException
    {
        const string DefaultMessage = "Document not found";

        public DocumentNotFoundException()
            : base(DefaultMessage) { }

        public DocumentNotFoundException(Exception? innerException)
            : base(DefaultMessage, innerException) { }

        public DocumentNotFoundException(string documentId)
            : base(string.IsNullOrEmpty(documentId) ? DefaultMessage : $"The document '{documentId}' was not found")
        {
            DocumentId = documentId;
        }

        public string? DocumentId { get; private set; }
    }
}
