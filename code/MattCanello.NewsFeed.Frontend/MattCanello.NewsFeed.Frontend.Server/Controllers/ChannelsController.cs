﻿using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.Frontend.Server.Controllers
{
    [Route("channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelRepository _channelRepository;

        public ChannelsController(IChannelRepository channelRepository) 
            => _channelRepository = channelRepository;

        [HttpGet("")]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Channel>))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var channels = await _channelRepository.GetAllAsync(cancellationToken);

            return Ok(channels);
        }
    }
}
