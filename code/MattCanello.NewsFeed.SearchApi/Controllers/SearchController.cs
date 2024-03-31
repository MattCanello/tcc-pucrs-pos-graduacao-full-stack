using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchApp _searchApp;

        public SearchController(ISearchApp searchApp)
        {
            _searchApp = searchApp;
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResponse<Document>))]
        public async Task<IActionResult> Search([FromBody, Required] SearchCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _searchApp.SearchAsync(command, cancellationToken);

            if (result.IsEmpty)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResponse<Document>))]
        public async Task<IActionResult> Search(
            string? q = null,
            [Range(1, Paging.MaxPagingSize)] int? size = null,
            [Range(0, int.MaxValue)] int? skip = null,
            [StringLength(100)] string? feedId = null,
            [StringLength(100)] string? channelId = null,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new SearchCommand(q, new Paging(skip, size), feedId, channelId);

            return await Search(command, cancellationToken);
        }
    }
}
