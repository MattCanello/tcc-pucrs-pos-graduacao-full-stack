using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed class Article
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public DateTimeOffset PublishDate { get; set; }

        public Thumbnail? Thumbnail { get; set; }

        [Required]
        public Channel? Channel { get; set; }

        [Required]
        public Feed? Feed { get; set; }

        public ArticleDetails? Details { get; set; }

        public ISet<Author>? Authors { get; set; }
    }
}
