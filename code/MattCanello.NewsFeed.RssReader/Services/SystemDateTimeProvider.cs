using MattCanello.NewsFeed.RssReader.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.RssReader.Services
{
    [ExcludeFromCodeCoverage]
    public sealed class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;
    }
}
