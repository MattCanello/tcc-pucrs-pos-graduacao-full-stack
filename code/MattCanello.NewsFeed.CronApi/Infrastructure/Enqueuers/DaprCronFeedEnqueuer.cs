﻿using Dapr.Client;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Enqueuers
{
    public sealed class DaprCronFeedEnqueuer : ICronFeedEnqueuer
    {
        const string BindingName = "rsspublishcommands";

        private readonly DaprClient _daprClient;
        private readonly IBindingRequestFactory _bindingRequestFactory;

        public DaprCronFeedEnqueuer(DaprClient daprClient, IBindingRequestFactory bindingRequestFactory)
        {
            _daprClient = daprClient;
            _bindingRequestFactory = bindingRequestFactory;
        }

        public async Task EnqueueFeedToProcessAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var bindingRequest = _bindingRequestFactory.CreateFeedEnqueueBindingRequest(feedId, BindingName);

            await _daprClient.InvokeBindingAsync(bindingRequest, cancellationToken);
        }
    }
}
