using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Models
{
    [Serializable]
    public sealed record ArticleDetails
    {
        public ArticleDetails() { }

        public ArticleDetails(string summary, params string[] lines)
        {
            Summary = summary;
            Lines = lines;
        }

        [Required]
        public string? Summary { get; set; }

        public IReadOnlyList<string>? Lines { get; set; }
    }
}
