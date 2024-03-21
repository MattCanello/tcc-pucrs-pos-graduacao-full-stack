using Microsoft.AspNetCore.Mvc.Filters;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Filters
{
    public sealed class HttpExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
