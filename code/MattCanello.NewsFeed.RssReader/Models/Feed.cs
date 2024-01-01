namespace MattCanello.NewsFeed.RssReader.Models
{
    public record Feed(string ChannelId, string FeedId, string Url, string? ETag = null, DateTimeOffset? LastModifiedDate = null);
}
