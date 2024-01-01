using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Models
{
    [Serializable]
    public sealed class Feed
    {
        public Feed() { }

        public Feed(string channelId, string feedId, string url)
        {
            this.ChannelId = channelId;
            this.FeedId = feedId;
            this.Url = url;
        }

        public string ChannelId { get; set; }

        public string FeedId { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        public string? LastETag { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
