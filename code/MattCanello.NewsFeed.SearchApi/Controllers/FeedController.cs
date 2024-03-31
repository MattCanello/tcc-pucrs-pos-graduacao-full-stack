using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IFeedApp _feedRepository;

        public FeedController(IFeedApp feedRepository)
        {
            _feedRepository = feedRepository;
        }

        [HttpGet("feed/recent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedResponse))]
        public async Task<IActionResult> GetRecent(
            [FromQuery, Range(1, GetFeedCommand.MaxPageSize), DefaultValue(GetFeedCommand.DefaultSize)] int? size = GetFeedCommand.DefaultSize, 
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new GetFeedCommand(size: size);

            var results = await _feedRepository.GetFeedAsync(command, cancellationToken);

            if (results.IsEmpty)
                return NoContent();

            return Ok(results);
        }

        [HttpGet("feed/{feedId}/recent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedResponse))]
        public async Task<IActionResult> GetRecent(
            [FromRoute, Required] string feedId,
            [FromQuery, Range(1, GetFeedCommand.MaxPageSize), DefaultValue(GetFeedCommand.DefaultSize)] int? size = GetFeedCommand.DefaultSize, 
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new GetFeedCommand(feedId, size);

            var results = await _feedRepository.GetFeedAsync(command, cancellationToken);

            if (results.IsEmpty)
                return NoContent();

            return Ok(results);
        }
    }
}
