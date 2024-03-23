﻿using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedEventPublisher : IEventPublisher
    {
        private readonly Action<Feed> _onPublish;

        public MockedEventPublisher(Action<Feed> onPublish) 
            => _onPublish = onPublish ?? throw new ArgumentNullException(nameof(onPublish));

        public Task PublishFeedCreatedEventAsync(Feed feed, CancellationToken cancellationToken = default)
        {
            _onPublish(feed);

            return Task.CompletedTask;
        }
    }
}
