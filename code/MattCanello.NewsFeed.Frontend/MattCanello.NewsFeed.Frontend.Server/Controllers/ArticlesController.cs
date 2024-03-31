using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Controllers
{
    [Route("articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleApp _articleApp;

        public ArticlesController(IArticleApp articleApp)
            => _articleApp = articleApp;

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Article>))]
        public async Task<IActionResult> GetFrontPage(CancellationToken cancellationToken = default)
        {
            var articles = await _articleApp.GetFrontPageArticlesAsync(cancellationToken);

            return this.Ok(articles);
        }

        [HttpGet("{feedId}/{articleId}")]
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

            return this.Ok(article);
        }

        [HttpGet("channel/{channelId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Article>))]
        public async Task<IActionResult> GetChannelArticles(
            [FromRoute, Required, StringLength(100)] string channelId,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articles = await _articleApp.GetChannelArticlesAsync(channelId, cancellationToken);

            return this.Ok(articles);
        }
    }
}
