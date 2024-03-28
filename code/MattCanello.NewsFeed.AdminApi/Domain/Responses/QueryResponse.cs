using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Responses
{
    [Serializable]
    public sealed record QueryResponse<TModel> where TModel : class, new()
    {
        public static readonly QueryResponse<TModel> Empty = new QueryResponse<TModel>(0, Array.Empty<TModel>());

        public QueryResponse(long total, IEnumerable<TModel> items)
        {
            Total = total;
            Items = items.ToArray();
        }

        [Required]
        [DefaultValue(0)]
        public long Total { get; init; }

        [Required]
        public IReadOnlyCollection<TModel> Items { get; init; } = Array.Empty<TModel>();
    }
}
