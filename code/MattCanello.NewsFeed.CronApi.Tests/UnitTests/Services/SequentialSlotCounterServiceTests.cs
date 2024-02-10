using MattCanello.NewsFeed.CronApi.Domain.Services;
using MattCanello.NewsFeed.CronApi.Tests.Stubs;

namespace MattCanello.NewsFeed.CronApi.Tests.UnitTests.Services
{
    public class SequentialSlotCounterServiceTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(58)]
        public async Task GetNextSlotAsync_GivenSlotLowerThan59_ShouldReturnSlotPlus1(byte slot)
        {
            var service = new SequentialSlotCounterService(new SlotRepositoryStub(slot));

            var next = await service.GetNextSlotAsync();

            Assert.Equal((byte)slot + 1, next);
        }

        [Theory]
        [InlineData(59)]
        [InlineData(60)]
        [InlineData(byte.MaxValue)]
        public async Task GetNextSlotAsync_Given59OrMore_ShouldReturn0(byte slot)
        {
            var service = new SequentialSlotCounterService(new SlotRepositoryStub(slot));

            var next = await service.GetNextSlotAsync();

            Assert.Equal(0, next);
        }
    }
}
