﻿namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models
{
    [Serializable]
    public sealed record EntryElasticSearchModel
    {
        public string? EntryId { get; set; }

        public string? Title { get; set; }

        public string? Url { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public ISet<Category>? Categories { get; set; }

        public IReadOnlyList<Thumbanil>? Thumbnails { get; set; }

        public ISet<Author>? Authors { get; set; }

        public sealed record Category(string? Name);

        public sealed record Thumbanil(string? Url, int? Width = null, string? Credit = null);

        public sealed record Author(string? Name = null, string? Email = null, string? Url = null);
    }
}
