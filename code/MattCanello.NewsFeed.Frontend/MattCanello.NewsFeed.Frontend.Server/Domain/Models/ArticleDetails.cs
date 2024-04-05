using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed record ArticleDetails
    {
        [Required]
        public string? Summary { get; set; }

        public IReadOnlyList<string>? Lines { get; set; }
    }
}
