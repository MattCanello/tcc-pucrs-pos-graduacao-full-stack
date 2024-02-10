using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.CronApi.Controllers
{
    [Route("api/slot")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly IFeedRepository _feedRepository;

        public SlotController(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }

        [HttpGet("{slot}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlySet<string>))]
        public async Task<IActionResult> Get([Range(0,59)] byte slot, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            var feeds = await _feedRepository.GetFeedIdsAsync(slot, cancellationToken);

            return Ok(feeds);
        }

        [HttpGet("{slot}/{feedId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Feed))]
        public async Task<IActionResult> GetFeed([Range(0, 59)] byte slot, [StringLength(100)] string feedId, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            var feed = await _feedRepository.GetFeedAsync(slot, feedId, cancellationToken);

            if (feed is null)
                return NotFound();

            return Ok(feed);
        }
    }
}
