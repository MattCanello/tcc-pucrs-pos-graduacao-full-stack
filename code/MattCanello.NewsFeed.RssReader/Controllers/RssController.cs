﻿using CloudNative.CloudEvents;
using MattCanello.NewsFeed.Cross.CloudEvents.Extensions;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Process(CloudEvent cloudEvent, CancellationToken cancellationToken = default)
        {
            if (cloudEvent is null)
                return BadRequest();

            var feedId = cloudEvent.GetFeedId();

            if (string.IsNullOrEmpty(feedId))
                return BadRequest();

            await _rssService.ProcessFeedAsync(feedId, cancellationToken);

            return NoContent();
        }
    }
}
