using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Commands
{
    [Serializable]
    public sealed class CreateChannelCommand
    {
        [Required]
        [StringLength(100)]
        public string? ChannelId { get; set; }

        [Required]
        public ChannelData? Data { get; set; }
    }
}
