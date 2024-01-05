using AutoMapper;
using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Repositories;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.RssReader.Controllers
{
    [Route("api/feed")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IMapper _mapper;

        public FeedController(IFeedRepository feedRepository, IMapper mapper)
        {
            _feedRepository = feedRepository;
            _mapper = mapper;
        }

        [HttpGet("{feedId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Feed))]
        public async Task<IActionResult> Get(string feedId, CancellationToken cancellationToken = default)
        {
            var feed = await _feedRepository.GetAsync(feedId, cancellationToken);

            if (feed is null)
                return NotFound();

            return Ok(feed);
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Feed))]
        public async Task<IActionResult> Create(CreateFeedCommand message)
        {
            if (message is null)
                return BadRequest();

            var feed = _mapper.Map<Feed>(message);
            await _feedRepository.UpdateAsync(feed.FeedId, feed);

            return CreatedAtAction(nameof(Get), new { feedId = feed.FeedId }, feed);
        }

        [HttpDelete("{feedId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string feedId, CancellationToken cancellationToken = default)
        {
            await _feedRepository.DeleteAsync(feedId, cancellationToken);
            return NoContent();
        }
    }
}
