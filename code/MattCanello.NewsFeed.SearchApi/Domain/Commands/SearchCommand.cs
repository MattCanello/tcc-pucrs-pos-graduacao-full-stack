using MattCanello.NewsFeed.SearchApi.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Commands
{
    [Serializable]
    public sealed class SearchCommand
    {
        [Required]
        public string? Query { get; set; }

        public Paging? Paging { get; set; }

        public string? FeedId { get; set; }
    }
}
