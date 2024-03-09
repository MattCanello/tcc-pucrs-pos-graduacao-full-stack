using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IEntryRepository _entryRepository;

        public DocumentController(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
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
