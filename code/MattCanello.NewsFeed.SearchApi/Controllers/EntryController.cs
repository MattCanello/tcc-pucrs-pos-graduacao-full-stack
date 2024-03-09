using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IEntryRepository _entryRepository;

        public EntryController(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        [HttpGet("feed/{feedId}/entry/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Entry))]
        public async Task<IActionResult> GetById([FromRoute, Required] string feedId, [FromRoute, Required] string id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entry = await _entryRepository.GetByIdAsync(feedId, id, cancellationToken);

            if (entry is null)
                return NotFound();

            return Ok(entry);
        }
    }
}
