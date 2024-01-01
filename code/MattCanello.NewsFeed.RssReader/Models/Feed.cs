using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Models
{
    [Serializable]
    public sealed class Feed
    {
        public string ChannelId { get; set; }

        public string FeedId { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        public string? LastETag { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
