using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Factories
{
    public sealed class ArticleDetailsFactory : IArticleDetailsFactory
    {
        public ArticleDetails? FromDescription(string? description)
        {
            if (string.IsNullOrEmpty(description))
                return null;

            var lines = description
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
