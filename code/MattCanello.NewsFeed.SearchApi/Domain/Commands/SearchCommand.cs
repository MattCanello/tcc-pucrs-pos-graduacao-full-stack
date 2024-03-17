using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Domain.Commands
{
    [Serializable]
    public sealed record SearchCommand
    {
        public SearchCommand() { }

        public SearchCommand(string? query = null, Paging? paging = null, string? feedId = null) 
        {
            Query = query;
            Paging = paging;
            FeedId = feedId;
        }

        public string? Query { get; set; }

        public Paging? Paging { get; set; }

        public string? FeedId { get; set; }
    }
}
