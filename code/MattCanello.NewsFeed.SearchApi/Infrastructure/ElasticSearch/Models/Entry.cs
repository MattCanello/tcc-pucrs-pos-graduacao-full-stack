namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models
{
    [Serializable]
    public sealed record Entry
    {
        public string? EntryId { get; set; }

        public string? Title { get; set; }

        public string? Url { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public ISet<Category>? Categories { get; set; }

        public IReadOnlyList<Thumbanil>? Thumbnails { get; set; }

        public ISet<Author>? Authors { get; set; }

        [Serializable]
        public sealed record Category()
        {
            public string? Name { get; set; }
        }

        [Serializable]
        public sealed record Thumbanil(string? Url = null, int? Width = null, string? Credit = null);

        [Serializable]
        public sealed record Author(string? Name = null, string? Email = null, string? Url = null);
    }
}
