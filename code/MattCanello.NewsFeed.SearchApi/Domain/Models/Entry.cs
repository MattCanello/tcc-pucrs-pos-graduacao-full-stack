using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public sealed class Entry
    {
        public Entry()
        {
            Categories = new HashSet<Category>();
            Thumbnails = Array.Empty<Thumbnail>();
            Authors = Array.Empty<Author>();
        }

        [Required]
        public string? Id { get; set; }

        public string? Title { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string? Url { get; set; }

        [DataType(DataType.Html)]
        public string? Description { get; set; }

        public ISet<Category> Categories { get; set; }

        [Required]
        public DateTimeOffset PublishDate { get; set; }

        public IList<Thumbnail> Thumbnails { get; set; }

        public IList<Author> Authors { get; set; }
    }
}
