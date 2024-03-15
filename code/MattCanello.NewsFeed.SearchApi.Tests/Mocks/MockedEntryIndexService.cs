using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Tests.Mocks
{
    internal sealed class MockedEntryIndexService : IEntryIndexService
    {
        private readonly IDictionary<string, IndexEntryCommand> _commands;

        public MockedEntryIndexService()
        {
            _commands = new Dictionary<string, IndexEntryCommand>();
        }

        internal IDictionary<string, IndexEntryCommand> IndexedCommands => _commands;

        public Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            var id = DateTimeOffset.Now.Ticks.ToString("F0");

            _commands[id] = command;

            return Task.FromResult(id);
        }
    }
}
