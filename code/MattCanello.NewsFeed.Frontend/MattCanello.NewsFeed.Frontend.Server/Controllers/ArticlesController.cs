using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.Frontend.Server.Controllers
{
    [Route("articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleApp _articleRepository;

        public ArticlesController(IArticleApp articleRepository)
            => _articleRepository = articleRepository;

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Article>))]
        public async Task<IActionResult> GetFrontPage()
        {
            var articles = await _articleRepository.GetFrontPageArticlesAsync();

            return this.Ok(articles);
        }
    }
}
