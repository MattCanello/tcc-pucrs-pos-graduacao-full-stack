using CloudNative.CloudEvents;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.RssReader.Controllers
{
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly IRssApp _rssService;

        public RssController(IRssApp rssService)
        {
            _rssService = rssService;
        }

        [Obsolete]
        [HttpPost("api/rss/{feedId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Process(string feedId, CancellationToken cancellationToken = default)
        {
            await _rssService.ProcessFeedAsync(feedId, cancellationToken);
            return NoContent();
        }

        [HttpPost("processrss")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Process(CloudEvent cloudEvent, CancellationToken cancellationToken = default)
        {
            if (cloudEvent is null)
                return BadRequest();

            var feedId = cloudEvent.Subject;
            if (string.IsNullOrEmpty(feedId))
                return BadRequest();

            await _rssService.ProcessFeedAsync(feedId, cancellationToken);

            return NoContent();
        }
    }
}
