using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class EntryReader : IEntryReader
    {
        public IEnumerable<Entry> FromRSS(SyndicationFeed syndicationFeed)
        {
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            foreach (var item in syndicationFeed.Items)
            {
                var authors = item
                    .Authors
                    ?.Select(author => new Author(author.Name, author.Email, author.Uri));

                var categories = item.Categories
                    ?.Select(category => new Category(category.Name, category.Label, category.Scheme));

                var entryItem = new Entry()
                {
                    Description = item.Summary?.Text?.Trim(),
                    Id = item.Id,
                    PublishDate = item.PublishDate,
                    Title = item.Title?.Text,
                    Url = item.Links?.FirstOrDefault()?.Uri.ToString(),
                    Authors = EvaluateAuthors(item).Distinct().ToList(),
                    Thumbnails = new List<Thumbnail>()
                };

                foreach (var category in BuildCategories(item.Categories))
                    entryItem.Categories.Add(category);

                yield return entryItem;
            }
        }

        private static IEnumerable<Author> EvaluateAuthors(SyndicationItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            foreach (var author in ToAuthor(item.Authors))
                yield return author;

            foreach (var contributor in ToAuthor(item.Contributors))
                yield return contributor;
        }

        private static IEnumerable<Author> ToAuthor(IEnumerable<SyndicationPerson>? authors)
        {
            if (authors is null)
                yield break;

            foreach (var author in authors)
                yield return new Author(author.Name, author.Email, author.Uri);
        }

        private static IEnumerable<Category> BuildCategories(IEnumerable<SyndicationCategory>? categories)
        {
            if (categories is null)
                yield break;

            foreach (var category in categories)
                yield return new Category(category.Name, category.Label, category.Scheme);
        }
    }
}
