using MattCanello.NewsFeed.CronApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Domain.Applications
{
    public sealed class CronPublishAppLog : ICronPublishApp
    {
        private readonly ILogger _logger;
        private readonly CronPublishApp _innerApp;

        public CronPublishAppLog(CronPublishApp innerApp, ILogger<ICronPublishApp> logger)
        {
            _innerApp = innerApp;
            _logger = logger;
        }

        public async Task<int> PublishSlotAsync(byte slot, CancellationToken cancellationToken = default)
        {
            using var scope = _logger.BeginScope(new { slot });

            _logger.LogInformation("Starting publishing slot {slot}", slot);

            var count = await _innerApp.PublishSlotAsync(slot, cancellationToken);

            _logger.LogInformation("{count} feeds published for slot {slot}", count, slot);

            return count;
        }
    }
}
