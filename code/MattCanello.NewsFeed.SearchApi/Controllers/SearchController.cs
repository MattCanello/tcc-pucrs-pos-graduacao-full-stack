using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResponse<Document>))]
        public async Task<IActionResult> Search([FromQuery] SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _searchApp.SearchAsync(searchCommand, cancellationToken);

            if (result.IsEmpty)
                return NoContent();

            return Ok(result);
        }
    }
}
