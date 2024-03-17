using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Tests.Mocks
{
    sealed class NoEntryIndexPolicy : IEntryIndexPolicy
    {
        public Task EvaluateAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
