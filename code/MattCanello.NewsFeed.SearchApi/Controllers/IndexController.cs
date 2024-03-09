using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IIndexApp _indexApp;

        public IndexController(IIndexApp indexApp)
        {
            _indexApp = indexApp;
        }

        [HttpPost("index")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Document))]
        public async Task<IActionResult> Index([FromBody, Required] IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _indexApp.IndexAsync(command, cancellationToken);

            var doc = new Document(id, command.FeedId!, command.Entry!);

            return CreatedAtAction("GetById", "Document", new { feedId = command.FeedId, id }, doc);
        }
    }
}
