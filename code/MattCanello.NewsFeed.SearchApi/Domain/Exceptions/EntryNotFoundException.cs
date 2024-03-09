namespace MattCanello.NewsFeed.SearchApi.Domain.Exceptions
{
    [Serializable]
    public class EntryNotFoundException : ApplicationException
    {
        const string DefaultMessage = "Entry not found";

        public EntryNotFoundException()
            : base(DefaultMessage) { }

        public EntryNotFoundException(Exception? innerException)
            : base(DefaultMessage, innerException) { }

        public EntryNotFoundException(string entryId)
            : base(string.IsNullOrEmpty(entryId) ? DefaultMessage : $"The entry '{entryId}' was not found")
        {
            EntryId = entryId;
        }

        public string? EntryId { get; private set; }
    }
}
