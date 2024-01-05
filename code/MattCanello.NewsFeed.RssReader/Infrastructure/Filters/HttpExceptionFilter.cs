using MattCanello.NewsFeed.RssReader.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Filters
{
    public sealed class HttpExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is FeedNotFoundException)
            {
                context.Exception = null;
                context.Result = new NotFoundResult();
                return;
            }

            base.OnActionExecuted(context);
        }
    }
}
