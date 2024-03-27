using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Controllers
{
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly ICreateFeedApp _createFeedApp;
        private readonly IUpdateFeedApp _updateFeedApp;
        private readonly IFeedRepository _feedRepository;

        public FeedController(ICreateFeedApp createFeedApp, IUpdateFeedApp updateFeedApp, IFeedRepository feedRepository)
        {
            _createFeedApp = createFeedApp;
            _updateFeedApp = updateFeedApp;
            _feedRepository = feedRepository;
        }

        [HttpGet("feed/{feedId}")]
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

        [HttpPost("create-feed")]
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

        [HttpPost("update-feed")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Feed))]
        public async Task<IActionResult> UpdateFeed([FromBody, Required] UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var feed = await _updateFeedApp.UpdateFeedAsync(command, cancellationToken);

            return Ok(feed);
        }
    }
}
