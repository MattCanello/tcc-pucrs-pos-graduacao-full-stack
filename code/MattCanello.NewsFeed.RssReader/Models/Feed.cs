using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Models
{
    [Serializable]
    public sealed class Feed
    {
        public Feed() { }

        public Feed(string feedId, string url, string? lastETag = null, DateTimeOffset? lastModifiedDate = null)
        {
            this.FeedId = feedId;
            this.Url = url;
            this.LastETag = lastETag;
            this.LastModifiedDate = lastModifiedDate;
        }

        public string FeedId { get; set; }

        [Url]
        [DataType(DataType.Url)]
        public string Url { get; set; }

        public string? LastETag { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
