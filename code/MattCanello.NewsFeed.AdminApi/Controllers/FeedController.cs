using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Controllers
{
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IFeedApp _feedApp;
        private readonly IFeedRepository _feedRepository;

        public FeedController(IFeedApp feedApp, IFeedRepository feedRepository)
        {
            _feedApp = feedApp;
            _feedRepository = feedRepository;
        }

        [HttpGet("feed/{feedId}")]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedWithChannel))]
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FeedWithChannel))]
        public async Task<IActionResult> Create([FromBody] CreateFeedCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var feed = await _feedApp.CreateFeedAsync(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { feedId = command.FeedId }, feed);
        }

        [HttpPost("update-feed")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedWithChannel))]
        public async Task<IActionResult> UpdateFeed([FromBody, Required] UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var feed = await _feedApp.UpdateFeedAsync(command, cancellationToken);

            return Ok(feed);
        }

        [HttpGet("feed")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryResponse<Feed>))]
        public async Task<IActionResult> Query([FromQuery]QueryCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _feedRepository.QueryAsync(command, cancellationToken);

            if (response.Total == 0)
                return NoContent();

            return Ok(response);
        }
    }
}
