using MattCanello.NewsFeed.CronApi.Domain.Commands;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.CronApi.Controllers
{
    [Route("api/feed")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IRegisterFeedHandler _registerFeedHandler;

        public FeedController(IRegisterFeedHandler registerFeedHandler)
        {
            _registerFeedHandler = registerFeedHandler;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Feed))]
        public async Task<IActionResult> Register(RegisterFeedCommand command, CancellationToken cancellationToken = default)
        {
            if (command is null)
                return BadRequest();

            var result = await _registerFeedHandler.RegisterFeedAsync(command, cancellationToken);

            var createdAtActionRouteValues = new { slot = result.Slot, feedId = result.Feed.FeedId };
            return CreatedAtAction(actionName: nameof(SlotController.GetFeed), controllerName: nameof(SlotController), routeValues: createdAtActionRouteValues, result.Feed);
        }
    }
}
