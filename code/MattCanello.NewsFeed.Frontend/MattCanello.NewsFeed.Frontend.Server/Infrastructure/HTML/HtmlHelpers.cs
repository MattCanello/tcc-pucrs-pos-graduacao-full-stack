using System.Text.RegularExpressions;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.HTML
{
    public static class HtmlHelpers
    {
        static readonly Regex TagToBreakLinePattern = new Regex(@"<br *\/?>|<\/?p>|<\/?ol>|<\/?ul>|<\/?li>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static readonly Regex EverythingBetweenQuotesPattern = new Regex(@"\w+=([""'])(?:(?=(\\?))\2.)*?\1", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static readonly Regex AnchorPattern = new Regex(@"<a *>|<\/a>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static readonly Regex TagToSupressPattern = new Regex(@"<\/?\w+>|<\w+ *\/?>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static readonly Regex DivTagPattern = new Regex(@"(?<div><div(.*?)<\/div>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static string StripHtmlTags(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            html = DivTagPattern.Replace(html, " ");

            html = EverythingBetweenQuotesPattern.Replace(html, string.Empty);

            html = AnchorPattern.Replace(html, string.Empty);

            html = TagToBreakLinePattern.Replace(html, Environment.NewLine);

            html = TagToSupressPattern.Replace(html, string.Empty);

            return html.Trim();
        }
    }
}
