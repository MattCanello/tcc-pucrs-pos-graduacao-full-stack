using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelApp _channelApp;

        public ChannelController(IChannelApp channelApp)
        {
            _channelApp = channelApp;
        }

        [HttpGet("channel/{channelId}/recent")]
        public async Task<IActionResult> GetRecent(
            [FromRoute, Required, StringLength(100)] string channelId,
            [FromQuery, Range(1, GetChannelDocumentsCommand.MaxPageSize), DefaultValue(GetChannelDocumentsCommand.DefaultSize)] int? size = GetChannelDocumentsCommand.DefaultSize,
            [FromQuery, Range(0, int.MaxValue), DefaultValue(0)] int? skip = 0,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new GetChannelDocumentsCommand(channelId, size, skip);

            var results = await _channelApp.GetDocumentsAsync(command, cancellationToken);

            if (results.IsEmpty)
                return NoContent();

            return Ok(results);
        }
    }
}
