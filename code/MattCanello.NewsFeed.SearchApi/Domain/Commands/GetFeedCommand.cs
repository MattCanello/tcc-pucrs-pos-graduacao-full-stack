using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Commands
{
    [Serializable]
    public sealed record GetFeedCommand
    {
        public const int DefaultSize = 10;
        public const int MaxPageSize = 100;

        public GetFeedCommand()
            : this(null, null, null) { }

        public GetFeedCommand(string? feedId = null, int? size = DefaultSize, int? skip = 0)
        {
            FeedId = feedId;
            Size = size ?? DefaultSize;
            Skip = skip ?? 0;
        }

        public string? FeedId { get; set; }

        [Range(1, MaxPageSize)]
        [DefaultValue(DefaultSize)]
        public int? Size { get; set; } = DefaultSize;

        [Range(0, int.MaxValue)]
        [DefaultValue(0)]
        public int? Skip { get; set; } = 0;
    }
}
