using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Messages
{
    [Serializable]
    public sealed class ReadRssResponseMessage
    {
        public static ReadRssResponseMessage NotModified = new ReadRssResponseMessage() { IsNotModified = true };

        private ReadRssResponseMessage() { }

        public ReadRssResponseMessage(string? eTag, DateTimeOffset? responseDate)
        {
            ETag = eTag;
            ResponseDate = responseDate;
        }

        public ReadRssResponseMessage(SyndicationFeed? feed, string? eTag, DateTimeOffset? responseDate)
            : this(eTag, responseDate)
        {
            Feed = feed;
        }

        public SyndicationFeed? Feed { get; }

        public string? ETag { get; }

        public DateTimeOffset? ResponseDate { get; }

        public bool IsNotModified { get; private set; }
    }
}
