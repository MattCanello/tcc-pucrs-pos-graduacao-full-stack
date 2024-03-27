using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Commands
{
    [Serializable]
    public sealed class UpdateFeedCommand
    {
        [Required]
        public string? FeedId { get; set; }

        [Required]
        public ChannelData? Channel { get; set; }
    }
}
