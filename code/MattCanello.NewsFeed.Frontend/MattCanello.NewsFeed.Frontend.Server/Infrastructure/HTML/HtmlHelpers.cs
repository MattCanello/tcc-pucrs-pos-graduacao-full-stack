using System.Text.RegularExpressions;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.HTML
{
    public static class HtmlHelpers
    {
        static readonly Regex EndPAndEndLI = new Regex(@"<p>|<ul>|<li>|<\/p>|<\/li>|<\/ul>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static readonly Regex EverythingBetweenQuotesPattern = new Regex(@"\w+=([""'])(?:(?=(\\?))\2.)*?\1", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static readonly Regex AnchorPattern = new Regex(@"<a *>|<\/a>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static string StripHtmlTags(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            html = EverythingBetweenQuotesPattern.Replace(html, string.Empty);

            html = AnchorPattern.Replace(html, string.Empty);

            html = EndPAndEndLI.Replace(html, Environment.NewLine);

            return html;
        }
    }
}
