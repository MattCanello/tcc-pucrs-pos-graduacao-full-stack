using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.CronApi.Controllers
{
    [Route("api/cron")]
    [ApiController]
    public class CronController : ControllerBase
    {
        private readonly ICronPublishApp _cronPublishApp;

        public CronController(ICronPublishApp cronPublishApp)
        {
            _cronPublishApp = cronPublishApp;
        }

        [HttpPost("publish/{slot}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Publish([Range(0, 59)] byte slot)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            await _cronPublishApp.PublishSlotAsync(slot);

            return NoContent();
        }
    }
}
