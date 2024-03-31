using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Domain.Commands
{
    [Serializable]
    public sealed record SearchCommand
    {
        public SearchCommand() { }

        public SearchCommand(string? query = null, Paging? paging = null, string? feedId = null, string? channelId = null) 
        {
            Query = query;
            Paging = paging;
            FeedId = feedId;
            ChannelId = channelId;
        }

        public string? Query { get; set; }

        public Paging? Paging { get; set; }

        public string? FeedId { get; set; }

        public string? ChannelId { get; set; }
    }
}
