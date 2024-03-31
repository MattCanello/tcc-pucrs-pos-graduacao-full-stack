using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public sealed record Paging
    {
        public const int DefaultSize = 10;
        public const int MaxPageSize = 100;

        public Paging()
            : this(0, DefaultSize)
        {
        }

        public Paging(int skip = 0, int size = DefaultSize)
        {
            Skip = skip;
            Size = size;
        }

        public Paging(int? skip = null, int? size = null)
            : this(skip ?? 0, size ?? DefaultSize) { }

        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int Skip { get; init; } = 0;

        [Required]
        [DefaultValue(DefaultSize)]
        [Range(0, MaxPageSize)]
        public int Size { get; init; } = DefaultSize;
    }
}
