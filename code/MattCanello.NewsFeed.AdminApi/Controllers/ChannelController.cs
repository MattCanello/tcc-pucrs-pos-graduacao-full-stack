using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Controllers
{
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IChannelApp _channelApp;

        public ChannelController(IChannelRepository channelRepository, IChannelApp channelApp)
        {
            _channelRepository = channelRepository;
            _channelApp = channelApp;
        }

        [ResponseCache(Duration = 60)]
        [HttpGet("channel/{channelId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Channel))]
        public async Task<IActionResult> GetById([FromRoute, Required] string channelId, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var channel = await _channelRepository.GetByIdAsync(channelId, cancellationToken);

            if (channel is null)
                return NotFound();

            return Ok(channel);
        }

        [HttpPost("create-channel")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Channel))]
        public async Task<IActionResult> Create([FromBody, Required] CreateChannelCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var channel = await _channelApp.CreateAsync(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { channelId = command.ChannelId }, channel);
        }

        [HttpPost("update-channel")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Channel))]
        public async Task<IActionResult> Update([FromBody, Required] UpdateChannelCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var channel = await _channelApp.UpdateAsync(command, cancellationToken);

            return Ok(channel);
        }

        [HttpGet("channel")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryResponse<Channel>))]
        public async Task<IActionResult> Query([FromQuery] QueryCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _channelRepository.QueryAsync(command, cancellationToken);

            if (response.Total == 0)
                return NoContent();

            return Ok(response);
        }
    }
}
