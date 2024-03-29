using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.Frontend.Server.Controllers
{
    [Route("articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;

        public ArticlesController(IArticleRepository articleRepository)
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
