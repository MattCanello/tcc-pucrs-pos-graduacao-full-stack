using Dapr.Client;
using MattCanello.NewsFeed.CronApi.Domain.Constraints;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Repositories
{
    public sealed class DaprSlotRepository : ISlotRepository
    {
        const string StoreName = "cronstatestore";
        const string CurrentSlotKey = "_current-slot";

        private readonly DaprClient _daprClient;

        public DaprSlotRepository(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<byte> GetCurrentSlotAsync(CancellationToken cancellationToken = default)
        {
            var currentSlot = await _daprClient.GetStateAsync<byte?>(StoreName, CurrentSlotKey, cancellationToken: cancellationToken);

            return currentSlot ?? SlotConstants.SlotMaxValue;
        }

        public async Task SetCurrentSlotAsync(byte slot, CancellationToken cancellationToken = default)
        {
            await _daprClient.SaveStateAsync(StoreName, CurrentSlotKey, slot, cancellationToken: cancellationToken);
        }
    }
}
