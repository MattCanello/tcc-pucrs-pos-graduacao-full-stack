using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Mocks
{
    sealed class MockedChannelRepository : IChannelRepository
    {
        public Task<IEnumerable<Channel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Channel> mockedChannels = new List<Channel>()
            {
                new Channel("the-guardian", "The Guardian"),
                new Channel("g1", "G1"),
                new Channel("folha-spaulo", "Folha de S. Paulo"),
                new Channel("exame", "Exame"),
                new Channel("gzh", "GZH"),
                new Channel("the-verge", "The Verge"),
                new Channel("tecmundo", "Tecmundo"),
                new Channel("teccrunch", "TechCrunch"),
            };

            return Task.FromResult(mockedChannels);
        }
    }
}
