using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;

namespace MattCanello.NewsFeed.Cross.Abstractions.Tests.Mocks
{
    public sealed class MockedDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTimeOffset _now;

        public MockedDateTimeProvider(DateTimeOffset now) => _now = now;

        public DateTimeOffset GetUtcNow() => _now;
    }
}
