using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedCreateFeedApp : IFeedApp
    {
        private readonly Func<CreateFeedCommand, FeedWithChannel> _createFeedFunc;
        private readonly Func<UpdateFeedCommand, FeedWithChannel> _updateFeedFunc;

        public MockedCreateFeedApp(Func<CreateFeedCommand, FeedWithChannel> createFeedFunc, Func<UpdateFeedCommand, FeedWithChannel> updateFeedFunc)
        {
            _createFeedFunc = createFeedFunc ?? throw new ArgumentNullException(nameof(createFeedFunc));
            _updateFeedFunc = updateFeedFunc ?? throw new ArgumentNullException(nameof(updateFeedFunc));
        }

        public Task<FeedWithChannel> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            var feed = _createFeedFunc(createFeedCommand);

            return Task.FromResult(feed);
        }

        public Task<FeedWithChannel> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            var feed = _updateFeedFunc(command);

            return Task.FromResult(feed);
        }
    }
}
