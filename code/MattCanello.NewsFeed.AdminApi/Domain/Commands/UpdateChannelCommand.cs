using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Commands
{
    [Serializable]
    public sealed class UpdateChannelCommand
    {
        [Required]
        [StringLength(100)]
        public string? ChannelId { get; set; }

        [Required]
        public ChannelData? Data { get; set; }
    }
}
