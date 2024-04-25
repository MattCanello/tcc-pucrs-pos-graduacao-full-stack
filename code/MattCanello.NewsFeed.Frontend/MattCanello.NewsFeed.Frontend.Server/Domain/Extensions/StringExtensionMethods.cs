using System.Text.RegularExpressions;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Extensions
{
    public static class StringExtensionMethods
    {
        private static readonly Regex LastLinePattern = new Regex(@"(?<lastLine>([^\.!?]*[\.!?])\s*\.\.)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static string TrimEllipseLastLine(this string content)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            content = content.Trim();
           
            var lastLineMatches = LastLinePattern.Match(content);

            if (!lastLineMatches.Success)
                return content;

            var lastLineMatch = lastLineMatches.Groups["lastLine"];
            return content.Replace(lastLineMatch.Value, "").Trim();
        }
    }
}
