using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.CronApi.Controllers
{
    [ApiController]
    public class CronController : ControllerBase
    {
        private readonly ICronPublishApp _cronPublishApp;

        public CronController(ICronPublishApp cronPublishApp)
        {
            _cronPublishApp = cronPublishApp;
        }

        [HttpPost("publish-{slot}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Publish([FromRoute] byte slot = 0, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            await _cronPublishApp.PublishSlotAsync(slot, cancellationToken);

            return NoContent();
        }
    }
}
