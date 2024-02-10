using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;

namespace MattCanello.NewsFeed.Cross.Abstractions
{
    public sealed class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;
    }
}
