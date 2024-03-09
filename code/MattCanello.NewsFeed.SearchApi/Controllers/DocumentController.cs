using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IIndexApp _indexApp;
        private readonly IEntryRepository _entryRepository;

        public DocumentController(IIndexApp indexApp, IEntryRepository entryRepository)
        {
            _indexApp = indexApp;
            _entryRepository = entryRepository;
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

        [HttpGet("feed/{feedId}/document/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
        public async Task<IActionResult> GetById([FromRoute, Required] string feedId, [FromRoute, Required] string id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entry = await _entryRepository.GetByIdAsync(feedId, id, cancellationToken);

            var doc = new Document(id, feedId, entry);
            
            return Ok(doc);
        }
    }
}
