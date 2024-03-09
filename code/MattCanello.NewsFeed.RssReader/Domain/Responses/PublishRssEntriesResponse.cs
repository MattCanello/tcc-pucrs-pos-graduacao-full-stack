using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Responses
{
    [Serializable]
    public sealed record class PublishRssEntriesResponse
    {
        public PublishRssEntriesResponse(int publishedCount = 0, DateTimeOffset? mostRecentPublishDate = null)
        {
            if (publishedCount < 0)
                throw new ArgumentOutOfRangeException(nameof(publishedCount), publishedCount, "The published count must be greater or equal to zero.");

            PublishedCount = publishedCount;
            MostRecentPublishDate = mostRecentPublishDate;
        }

        public int PublishedCount { get; init; }

        public DateTimeOffset? MostRecentPublishDate { get; init; }

        public void UpdateFeed(Feed feed)
        {
            ArgumentNullException.ThrowIfNull(feed);

            feed.LastPublishedEntryDate = MostRecentPublishDate ?? feed.LastPublishedEntryDate;
        }
    }
}
