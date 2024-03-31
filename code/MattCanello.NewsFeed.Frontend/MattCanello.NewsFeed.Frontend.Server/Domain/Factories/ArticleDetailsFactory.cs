using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.HTML;
using System.Net;
using System.Text.RegularExpressions;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Factories
{
    public sealed class ArticleDetailsFactory : IArticleDetailsFactory
    {
        private static readonly Regex ContinueReadingPattern = new Regex(@"^continue reading|^leia mais|^see all", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public ArticleDetails? FromDescription(string? description)
        {
            if (string.IsNullOrEmpty(description))
                return null;

            var lines = description
                .StripHtmlTags()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(line => WebUtility.HtmlDecode(line))
                .Where(line => !IsContinueReading(line))
                .ToArray();

            if (lines.Length == 0)
                return null;

            return new ArticleDetails()
            {
                Summary = lines.First(),
                Lines = lines.Skip(1).ToArray()
            };
        }

        public bool IsContinueReading(string line)
            => ContinueReadingPattern.IsMatch(line);
    }
}
