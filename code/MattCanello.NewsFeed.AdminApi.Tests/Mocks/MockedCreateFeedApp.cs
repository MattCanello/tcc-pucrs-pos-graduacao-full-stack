using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedCreateFeedApp : ICreateFeedApp
    {
        private readonly Func<CreateFeedCommand, Feed> _createFeedFunc;

        public MockedCreateFeedApp(Func<CreateFeedCommand, Feed> createFeedFunc) 
            => _createFeedFunc = createFeedFunc ?? throw new ArgumentNullException(nameof(createFeedFunc));

        public Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            var feed = _createFeedFunc(createFeedCommand);

            return Task.FromResult(feed);
        }
    }
}
