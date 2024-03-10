using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Domain.Commands
{
    [Serializable]
    public sealed class SearchCommand
    {
        public string? Query { get; set; }

        public Paging? Paging { get; set; }

        public string? FeedId { get; set; }
    }
}
