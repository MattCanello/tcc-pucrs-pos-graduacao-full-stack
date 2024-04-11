using MattCanello.NewsFeed.Frontend.Server.Domain.Commands;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Controllers
{
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleApp _articleApp;
        private readonly INewEntryHandler _newEntryHandler;

        public ArticlesController(IArticleApp articleApp, INewEntryHandler newEntryHandler)
        {
            _articleApp = articleApp;
            _newEntryHandler = newEntryHandler;
        }

        [HttpGet("articles")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Article>))]
        public async Task<IActionResult> GetFrontPage(CancellationToken cancellationToken = default)
        {
            var articles = await _articleApp.GetFrontPageArticlesAsync(cancellationToken);

            return Ok(articles);
        }

        [ResponseCache(Duration = 3600)]
        [HttpGet("articles/{feedId}/{articleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Article))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetArticle(
            [FromRoute, Required, StringLength(100)] string feedId,
            [FromRoute, Required, StringLength(100)] string articleId,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var article = await _articleApp.GetArticleAsync(feedId, articleId, cancellationToken);

            if (article is null)
                return NotFound();

            return Ok(article);
        }

        [HttpGet("articles/channel/{channelId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Article>))]
        public async Task<IActionResult> GetChannelArticles(
            [FromRoute, Required, StringLength(100)] string channelId,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articles = await _articleApp.GetChannelArticlesAsync(channelId, cancellationToken);

            return Ok(articles);
        }

        [HttpPost("new-entry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewEntry([FromBody, Required] NewEntryFoundCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _newEntryHandler.HandleAsync(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Article>))]
        public async Task<IActionResult> Search([FromQuery, Required] string? q = null, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var searchResult = await _articleApp.SearchAsync(q, cancellationToken);

            return Ok(searchResult);
        }
    }
}
