using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Controllers
{
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IUpdateChannelApp _updateChannelApp;

        public ChannelsController(IUpdateChannelApp updateChannelApp)
        {
            _updateChannelApp = updateChannelApp;
        }

        [HttpPost("update-channel")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Channel))]
        public async Task<IActionResult> UpdateChannel([FromBody, Required] UpdateChannelCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var channel = await _updateChannelApp.UpdateChannelAsync(command, cancellationToken);

            return Ok(channel);
        }
    }
}
