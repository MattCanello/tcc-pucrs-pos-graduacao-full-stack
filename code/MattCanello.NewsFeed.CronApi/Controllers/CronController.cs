using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.CronApi.Controllers
{
    [Route("api/cron")]
    [ApiController]
    public class CronController : ControllerBase
    {
        [HttpPost("publish/{slot}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Publish(byte slot)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            // TODO: implementar.

            return NoContent();
        }
    }
}
