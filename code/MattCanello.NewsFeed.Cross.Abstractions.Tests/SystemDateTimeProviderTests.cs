namespace MattCanello.NewsFeed.Cross.Abstractions.Tests
{
    public sealed class SystemDateTimeProviderTests
    {
        [Fact]
        public void GetUtcNow_ShouldReturnUtcNow()
        {
            var provider = new SystemDateTimeProvider();
            var now = DateTimeOffset.UtcNow;

            var providerNow = provider.GetUtcNow();
            Assert.Equal(now.DateTime, providerNow.DateTime, TimeSpan.FromSeconds(0.5));
            Assert.Equal(TimeSpan.Zero, providerNow.Offset);
        }
    }
}
