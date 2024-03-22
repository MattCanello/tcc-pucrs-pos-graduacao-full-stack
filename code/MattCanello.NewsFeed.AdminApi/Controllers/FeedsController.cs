using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Controllers
{
    [Route("feeds")]
    [ApiController]
    public class FeedsController : ControllerBase
    {
        private readonly ICreateFeedApp _createFeedApp;
        private readonly IFeedRepository _feedRepository;

        public FeedsController(ICreateFeedApp createFeedApp, IFeedRepository feedRepository)
        {
            _createFeedApp = createFeedApp;
            _feedRepository = feedRepository;
        }

        [HttpGet("{feedId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Feed))]
        public async Task<IActionResult> GetById(
            [FromRoute, Required, StringLength(100)] string feedId,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var feed = await _feedRepository.GetByIdAsync(feedId, cancellationToken);

            if (feed is null)
                return NotFound();

            return Ok(feed);
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Feed))]
        public async Task<IActionResult> Create([FromBody] CreateFeedCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var feed = await _createFeedApp.CreateFeedAsync(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { feedId = command.FeedId }, feed);
        }
    }
}
