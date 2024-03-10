using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public sealed record Paging
    {
        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int Skip { get; init; } = 0;

        [Required]
        [DefaultValue(100)]
        [Range(0, 1000)]
        public int Size { get; init; } = 100;
    }
}
