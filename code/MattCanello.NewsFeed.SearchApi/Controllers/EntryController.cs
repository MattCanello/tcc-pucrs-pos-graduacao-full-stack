using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [Route("api/entry")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IIndexService _indexService;

        public EntryController(IIndexService indexService)
        {
            _indexService = indexService;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Index([FromBody, Required] IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryId = await _indexService.IndexAsync(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { entryId });
        }

        [HttpGet("{entryId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Entry))]
        public Task<IActionResult> GetById([FromRoute, Required]string entryId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
