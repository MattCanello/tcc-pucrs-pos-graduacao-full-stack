using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Index([FromBody, Required] IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryId = await _indexApp.IndexAsync(command, cancellationToken);

            return CreatedAtAction(nameof(EntryController.GetById), nameof(EntryController), new { entryId }, command.Entry);
        }
    }
}
