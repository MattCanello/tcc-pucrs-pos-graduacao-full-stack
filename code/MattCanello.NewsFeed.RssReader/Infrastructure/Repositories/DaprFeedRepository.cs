﻿using Dapr.Client;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Repositories;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Repositories
{
    public sealed class DaprFeedRepository : IFeedRepository
    {
        private readonly DaprClient _daprClient;
        private const string StateStoreName = "rssreaderstate";

        public DaprFeedRepository(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<Feed?> GetAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var feed = await _daprClient.GetStateAsync<Feed?>(StateStoreName, feedId,
                cancellationToken: cancellationToken);

            return feed;
        }

        public async Task UpdateAsync(string feedId, Feed feed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(feed);

            await _daprClient.SaveStateAsync(StateStoreName, feedId, feed, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string feedId, CancellationToken cancellationToken = default)
        {
            await _daprClient.DeleteStateAsync(StateStoreName, feedId, cancellationToken: cancellationToken);
        }
    }
}
