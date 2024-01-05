using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.RssReader.Domain.Models
{
    [Serializable]
    public sealed class Feed
    {
        [JsonConstructor]
        private Feed() { }

        public Feed(string feedId, string url, string? lastETag = null, DateTimeOffset? lastModifiedDate = null)
        {
            FeedId = feedId;
            Url = url;
            LastETag = lastETag;
            LastModifiedDate = lastModifiedDate;
        }

        public string FeedId { get; set; }

        [Url]
        [DataType(DataType.Url)]
        public string Url { get; set; }

        public string? LastETag { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }

        public void SetAsModified(string? etag = null, DateTimeOffset? modifiedDate = null)
        {
            LastETag = etag ?? LastETag;
            LastModifiedDate = modifiedDate ?? LastModifiedDate;
        }
    }
}
