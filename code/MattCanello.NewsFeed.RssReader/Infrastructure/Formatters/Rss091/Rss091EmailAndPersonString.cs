using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Formatters.Rss091
{
    [Serializable]
    public sealed record Rss091EmailAndPersonString
    {
        private static readonly Regex EMailRegex = new Regex(@"[\w\-\.]+@([\w-]+\.)+[\w-]{2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex DisplayNameRegex = new Regex(@"\((?<DisplayName>[^)]*)\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public Rss091EmailAndPersonString(string email, string? displayName = null)
        {
            Email = email;
            DisplayName = displayName;
        }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; init; }
        public string? DisplayName { get; init; }

        public static bool TryParse(string emailAndName, out Rss091EmailAndPersonString? parsed)
        {
            parsed = null;

            ArgumentNullException.ThrowIfNull(emailAndName);

            var emailMatch = EMailRegex.Match(emailAndName);

            if (!emailMatch.Success)
                return false;

            var displayNameMatch = DisplayNameRegex.Match(emailAndName);

            parsed = new Rss091EmailAndPersonString(emailMatch.Value,
                displayNameMatch.Success ? displayNameMatch.Groups["DisplayName"].Value : null);

            return true;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(DisplayName))
                return Email;

            return $"{Email} ({DisplayName})";
        }
    }
}
