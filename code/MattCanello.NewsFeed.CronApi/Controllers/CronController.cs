using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.CronApi.Controllers
{
    [Route("api/cron")]
    [ApiController]
    public class CronController : ControllerBase
    {
        [HttpPost("publish/{slot}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Publish([Range(0, 59)] byte slot)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            // TODO: implementar.

            return NoContent();
        }
    }
}
