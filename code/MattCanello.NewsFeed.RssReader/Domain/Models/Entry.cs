using MattCanello.NewsFeed.RssReader.Domain.Responses;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Domain.Models
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

        public string? Id { get; set; }

        public string? Title { get; set; }

        [DataType(DataType.Url)]
        public string? Url { get; set; }

        [DataType(DataType.Html)]
        public string? Description { get; set; }

        public ISet<Category> Categories { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public IList<Thumbnail> Thumbnails { get; set; }

        public IList<Author> Authors { get; set; }

        public void ApplyParsedContent(ParsedContent parsedContent)
        {
            ArgumentNullException.ThrowIfNull(parsedContent);

            if (string.IsNullOrEmpty(Description))
                Description = parsedContent.Description;

            if (parsedContent.Thumbnail is null)
                return;

            Thumbnails ??= new List<Thumbnail>(1);
            Thumbnails.Add(parsedContent.Thumbnail);
        }
    }
}
