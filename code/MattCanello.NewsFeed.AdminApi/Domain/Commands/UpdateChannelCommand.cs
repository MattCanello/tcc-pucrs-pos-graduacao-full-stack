using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Commands
{
    [Serializable]
    public sealed class UpdateChannelCommand
    {
        [Required]
        public string? FeedId { get; set; }

        [Required]
        public ChannelData? Channel { get; set; }

        [Serializable]
        public sealed class ChannelData
        {
            public string? Name { get; set; }
            public string? Url { get; set; }
            public string? Description { get; set; }
            public string? Language { get; set; }
            public string? Copyright { get; set; }
            public string? ImageUrl { get; set; }
        }
    }
}
