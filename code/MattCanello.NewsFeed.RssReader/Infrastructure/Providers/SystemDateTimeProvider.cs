using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Providers;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Providers
{
    [ExcludeFromCodeCoverage]
    public sealed class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;
    }
}
