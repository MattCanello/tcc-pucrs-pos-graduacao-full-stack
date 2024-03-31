using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.HTML;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Factories
{
    public sealed class ArticleDetailsFactory : IArticleDetailsFactory
    {
        public ArticleDetails? FromDescription(string? description)
        {
            if (string.IsNullOrEmpty(description))
                return null;

            var lines = description
                .StripHtmlTags()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (lines.Length == 0)
                return null;

            return new ArticleDetails()
            {
                Summary = lines.First(),
                Lines = lines.Skip(1).ToArray()
            };
        }
    }
}
