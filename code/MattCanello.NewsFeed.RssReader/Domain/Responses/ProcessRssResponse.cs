namespace MattCanello.NewsFeed.RssReader.Domain.Responses
{
    [Serializable]
    public record class ProcessRssResponse
    {
        public static readonly ProcessRssResponse NotModified 
            = new ProcessRssResponse() { IsNotModified = true };

        private ProcessRssResponse() { }

        public ProcessRssResponse(int publishedEntriesCount)
        {
            if (publishedEntriesCount < 0)
                throw new ArgumentOutOfRangeException(nameof(publishedEntriesCount), publishedEntriesCount, "The number of published entries shall be greater than or equal to zero.");
            
            PublishedEntriesCount = publishedEntriesCount;
        }

        public int PublishedEntriesCount { get; }
        public bool IsNotModified { get; private set; }
    }
}
