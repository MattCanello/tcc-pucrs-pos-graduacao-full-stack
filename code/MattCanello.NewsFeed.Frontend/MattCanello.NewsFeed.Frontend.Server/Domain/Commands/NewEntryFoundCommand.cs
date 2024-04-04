using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Commands
{
    [Serializable]
    public sealed class NewEntryFoundCommand
    {
        [Required]
        public string? DocumentId { get; set; }

        [Required]
        public string? FeedId { get; set; }

        public string? EntryId { get; set; }
    }
}
