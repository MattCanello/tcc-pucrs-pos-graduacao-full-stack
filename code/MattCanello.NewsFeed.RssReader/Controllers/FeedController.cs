using MattCanello.NewsFeed.RssReader.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.RssReader.Controllers
{
    [Route("api/feed")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IRssService _rssService;

        public FeedController(IRssService rssService)
        {
            _rssService = rssService;
        }

        [HttpPost("process/{feedId}")]
        public async Task<IActionResult> Process(string feedId, CancellationToken cancellationToken)
        {
            await _rssService.ProcessFeedAsync(feedId, cancellationToken);
            return NoContent();
        }
    }
}
