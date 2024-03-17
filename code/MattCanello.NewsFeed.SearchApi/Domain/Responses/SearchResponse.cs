using MattCanello.NewsFeed.SearchApi.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.SearchApi.Domain.Responses
{
    [Serializable]
    public sealed class SearchResponse<TModel> where TModel : class
    {
        public SearchResponse() { }

        public SearchResponse(IReadOnlyList<TModel> results, long total, Paging paging)
        {
            Results = results;
            Total = total;
            Paging = paging;
        }

        [Required]
        [Range(0, long.MaxValue)]
        public long Total { get; set; }

        [Required]
        public IReadOnlyList<TModel>? Results { get; set; }

        public Paging? Paging { get; set; }

        [JsonIgnore]
        public bool IsEmpty => Total == 0;

        public static SearchResponse<TModel> CreateEmpty(Paging? paging = null) => new SearchResponse<TModel>
        {
            Paging = paging
        };
    }
}
