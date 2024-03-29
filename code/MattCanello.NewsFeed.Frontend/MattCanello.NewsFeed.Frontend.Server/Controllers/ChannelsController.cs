using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Models;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Channel>))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var channels = await _channelRepository.GetAllAsync(cancellationToken);

            return Ok(channels);
        }
    }
}
