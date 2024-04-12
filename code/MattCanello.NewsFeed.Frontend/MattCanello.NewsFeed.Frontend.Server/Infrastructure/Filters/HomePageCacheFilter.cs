using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Filters
{
    sealed class HomePageCacheFilter : ActionFilterAttribute
    {
        private readonly IHomePageCachingService _homePageCachingService;

        public HomePageCacheFilter(IHomePageCachingService homePageCachingService)
        {
            _homePageCachingService = homePageCachingService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var lastArticleDate = await _homePageCachingService.GetLatestArticleDateAsync();
            if (DateTimeOffset.TryParse(context.HttpContext.Request.Headers.IfModifiedSince, out DateTimeOffset ifModifiedSince))
            {
                if (lastArticleDate != null && lastArticleDate <= ifModifiedSince)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
                    return;
                }
            }

            await next();

            if (lastArticleDate != null)
                context.HttpContext.Response.Headers.LastModified = lastArticleDate.Value.ToString("R");
        }
    }
}
