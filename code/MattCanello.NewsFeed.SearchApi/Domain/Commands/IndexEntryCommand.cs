using MattCanello.NewsFeed.SearchApi.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Commands
{
    [Serializable]
    public sealed record IndexEntryCommand
    {
        [Required]
        public string? FeedId { get; set; }

        [Required]
        public string? ChannelId { get; set; }

        [Required]
        public Entry? Entry { get; set; }
    }
}
