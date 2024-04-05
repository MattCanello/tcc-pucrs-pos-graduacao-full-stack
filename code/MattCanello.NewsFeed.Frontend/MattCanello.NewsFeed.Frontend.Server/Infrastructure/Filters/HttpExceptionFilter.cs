using MattCanello.NewsFeed.Frontend.Server.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Filters
{
    sealed class HttpExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is FeedNotFoundException feedNotFoundException)
            {
                context.Exception = null;
                context.ModelState.AddModelError("feedId", feedNotFoundException.Message);
                context.Result = new NotFoundResult();
                return;
            }

            base.OnActionExecuted(context);
        }
    }
}
