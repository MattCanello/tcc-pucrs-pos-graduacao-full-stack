using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Events
{
    [Serializable]
    public sealed class CreateFeedMessage
    {
        [Required]
        [StringLength(100)]
        public string FeedId { get; set; }

        [Url]
        [Required]
        [DataType(DataType.Url)]
        public string Url { get; set; }
    }
}
