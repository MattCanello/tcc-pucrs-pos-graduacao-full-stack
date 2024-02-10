using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Filters
{
    public sealed class HttpExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is SlotOutOfRangeException slotOutOfRangeException)
            {
                context.Exception = null;
                context.ModelState.AddModelError("slot", slotOutOfRangeException.Message);
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            base.OnActionExecuted(context);
        }
    }
}
