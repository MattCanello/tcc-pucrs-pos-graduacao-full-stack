using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly ISearchApp _searchApp;

        public FeedController(ISearchApp searchApp) 
            => _searchApp = searchApp;

        [HttpGet("feed/recent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResponse<Document>))]
        public async Task<IActionResult> GetRecent(
            [FromQuery, Range(1, Paging.MaxPageSize), DefaultValue(Paging.DefaultSize)] int? size = Paging.DefaultSize,
            [FromQuery, Range(0, int.MaxValue), DefaultValue(0)] int? skip = 0,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new SearchCommand(paging: new Paging(skip, size));

            var results = await _searchApp.SearchAsync(command, cancellationToken);

            if (results.IsEmpty)
                return NoContent();

            return Ok(results);
        }

        [HttpGet("feed/{channelId}/recent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResponse<Document>))]
        public async Task<IActionResult> GetRecent(
            [FromRoute, Required, StringLength(100)] string channelId,
            [FromQuery, Range(1, Paging.MaxPageSize), DefaultValue(Paging.DefaultSize)] int? size = Paging.DefaultSize,
            [FromQuery, Range(0, int.MaxValue), DefaultValue(0)] int? skip = 0,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new SearchCommand(paging: new Paging(skip, size), channelId: channelId);

            var results = await _searchApp.SearchAsync(command, cancellationToken);

            if (results.IsEmpty)
                return NoContent();

            return Ok(results);
        }
    }
}
