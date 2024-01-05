using MattCanello.NewsFeed.RssReader.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.RssReader.Controllers
{
    [Route("api/rss")]
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly IRssService _rssService;

        public RssController(IRssService rssService)
        {
            _rssService = rssService;
        }

        [HttpPost("{feedId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Process(string feedId, CancellationToken cancellationToken = default)
        {
            await _rssService.ProcessFeedAsync(feedId, cancellationToken);
            return NoContent();
        }
    }
}
