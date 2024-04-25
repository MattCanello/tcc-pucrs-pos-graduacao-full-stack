using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Enrichers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Parsers;

namespace MattCanello.NewsFeed.RssReader.Domain.Factories
{
    public sealed class EntryFactory : IEntryFactory
    {
        private readonly INonStandardEnricherEvaluator _nonStandardEnricherEvaluator;
        private readonly IContentParserEvaluator _contentParserEvaluator;

        public EntryFactory(INonStandardEnricherEvaluator nonStandardEnricherEvaluator, IContentParserEvaluator contentParserEvaluator)
        {
            _nonStandardEnricherEvaluator = nonStandardEnricherEvaluator;
            _contentParserEvaluator = contentParserEvaluator;
        }

        public IEnumerable<Entry> FromRSS(SyndicationFeed syndicationFeed)
        {
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            foreach (var item in syndicationFeed.Items.OrderBy(item => item.PublishDate))
            {
                var authors = item
                    .Authors
                    ?.Select(author => new Author(author.Name, author.Email, author.Uri));

                var categories = item.Categories
                    ?.Select(category => new Category(category.Name, category.Label, category.Scheme));

                var entry = new Entry()
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
                    entry.Categories.Add(category);

                ReadContent(item, entry);

                EnrichWithNonStandardData(item, entry);

                yield return entry;
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

        private void EnrichWithNonStandardData(SyndicationItem rssItem, Entry entry)
        {
            ArgumentNullException.ThrowIfNull(rssItem);
            ArgumentNullException.ThrowIfNull(entry);

            if (rssItem.ElementExtensions is null || rssItem.ElementExtensions.Count == 0)
                return;

            var namespaceAndElements = rssItem.ElementExtensions
                .GroupBy(extension => extension.OuterNamespace);

            foreach (var group in namespaceAndElements)
            {
                var enricher = _nonStandardEnricherEvaluator.Evaluate(group.Key);
                if (enricher is null)
                    continue;

                enricher.Enrich(entry, group.Select(g => g.GetObject<XElement>()));
            }
        }

        private void ReadContent(SyndicationItem rssItem, Entry entry)
        {
            ArgumentNullException.ThrowIfNull(rssItem);
            ArgumentNullException.ThrowIfNull(entry);

            var parser = _contentParserEvaluator.EvaluateParser(rssItem);
            if (parser is null)
                return;

            var parsedContent = parser.TryParse(rssItem.Content);
            if (parsedContent is null)
                return;

            entry.ApplyParsedContent(parsedContent);
        }
    }
}
