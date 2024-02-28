using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;
using MattCanello.NewsFeed.RssReader.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        [HttpPost("process")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessRssResponse))]
        public async Task<IActionResult> Process([FromBody, Required] ProcessRssFeedCommand command, CancellationToken cancellationToken = default)
        {
            if (command is null)
                return BadRequest();

            var feedId = command.FeedId;

            if (string.IsNullOrEmpty(feedId))
                return BadRequest();

            var response = await _rssService.ProcessFeedAsync(feedId, cancellationToken);

            return Ok(response);
        }
    }
}
