using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Providers;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    public sealed class MockedDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTimeOffset _mockedDate;

        public static readonly MockedDateTimeProvider Any = new MockedDateTimeProvider(DateTimeOffset.UtcNow);

        public MockedDateTimeProvider(DateTimeOffset mockedDate)
            => _mockedDate = mockedDate;

        public DateTimeOffset GetUtcNow() => _mockedDate;
    }
}
