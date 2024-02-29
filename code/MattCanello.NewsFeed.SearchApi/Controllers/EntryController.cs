using MattCanello.NewsFeed.SearchApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class EntryController : ControllerBase
    {
        [HttpGet("api/entry/{entryId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Entry))]
        public Task<IActionResult> GetById([FromRoute, Required]string entryId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
