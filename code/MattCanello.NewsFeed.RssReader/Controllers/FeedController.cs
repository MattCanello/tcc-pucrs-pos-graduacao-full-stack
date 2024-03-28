using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Handlers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Repositories;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.RssReader.Controllers
{
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ICreateFeedHandler _createFeedHandler;

        public FeedController(IFeedRepository feedRepository, ICreateFeedHandler createFeedHandler)
        {
            _feedRepository = feedRepository;
            _createFeedHandler = createFeedHandler;
        }

        [HttpGet("feed/{feedId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Feed))]
        public async Task<IActionResult> Get(string feedId, CancellationToken cancellationToken = default)
        {
            var feed = await _feedRepository.GetAsync(feedId, cancellationToken);

            if (feed is null)
                return NotFound();

            return Ok(feed);
        }

        [HttpPost("create-feed")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Feed))]
        public async Task<IActionResult> Create(CreateFeedCommand command)
        {
            if (command is null)
                return BadRequest();

            var feed = await _createFeedHandler.CreateFeedAsync(command);
            return CreatedAtAction(nameof(Get), new { feedId = feed.FeedId }, feed);
        }

        [HttpDelete("feed/{feedId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string feedId, CancellationToken cancellationToken = default)
        {
            await _feedRepository.DeleteAsync(feedId, cancellationToken);
            return NoContent();
        }
    }
}
