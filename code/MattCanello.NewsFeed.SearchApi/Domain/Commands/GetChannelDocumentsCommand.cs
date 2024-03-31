using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Commands
{
    [Serializable]
    public sealed record GetChannelDocumentsCommand
    {
        public const int DefaultSize = 10;
        public const int MaxPageSize = 100;

        public GetChannelDocumentsCommand()
            : this(string.Empty, null, null) { }

        public GetChannelDocumentsCommand(string channelId, int? size = DefaultSize, int? skip = 0)
        {
            ChannelId = channelId;
            Size = size ?? DefaultSize;
            Skip = skip ?? 0;
        }

        [Required]
        [StringLength(100)]
        public string ChannelId { get; set; }

        [Range(1, MaxPageSize)]
        [DefaultValue(DefaultSize)]
        public int? Size { get; set; } = DefaultSize;

        [Range(0, int.MaxValue)]
        [DefaultValue(0)]
        public int? Skip { get; set; } = 0;
    }
}
