using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Profiles;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Repositories;
using MattCanello.NewsFeed.Frontend.Server.Tests.Mocks;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Repositories
{
    public class AdminChannelRepositoryTests
    {
        [Theory, AutoData]
        public async Task GetAllAsync_GivenMoreThanOnePage_ShouldReturnAllClients(AdminChannel adminChannel1, AdminChannel adminChannel2)
        {
            var mapper = new MapperConfiguration(conf => conf.AddProfile<AdminProfile>()).CreateMapper();

            var channelConfig = new MockedChannelConfiguration(1);
            
            var adminClient = new MockedAdminClient((cmd) => new AdminQueryResponse<AdminChannel>() { Items = new List<AdminChannel>(capacity: 1) { cmd.Skip == 0 ? adminChannel1 : adminChannel2 }, Total = 2 });

            var repository = new AdminChannelRepository(adminClient, mapper, channelConfig);

            var result = await repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            Assert.Equal(2, result.Count());

            var ch1 = result.FirstOrDefault(r => r.ChannelId == adminChannel1.ChannelId);
            Assert.NotNull(ch1);

            var ch2 = result.FirstOrDefault(r => r.ChannelId == adminChannel2.ChannelId);
            Assert.NotNull(ch1);
        }
    }
}
