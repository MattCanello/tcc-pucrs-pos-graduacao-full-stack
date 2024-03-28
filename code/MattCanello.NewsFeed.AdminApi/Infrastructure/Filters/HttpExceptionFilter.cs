using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Filters
{
    public sealed class HttpExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.Exception = null;
                context.Result = new NotFoundResult();
                return;
            }

            if (context.Exception is FeedAlreadyExistsException feedAlreadyExistsException)
            {
                context.Exception = null;
                context.ModelState.AddModelError(nameof(CreateFeedCommand.FeedId), feedAlreadyExistsException.Message);
                context.Result = new ConflictObjectResult(context.ModelState);
                return;
            }

            if (context.Exception is ChannelAlreadyExistsException channelAlreadyExistsException)
            {
                context.Exception = null;
                context.ModelState.AddModelError(nameof(CreateChannelCommand.ChannelId), channelAlreadyExistsException.Message);
                context.Result = new ConflictObjectResult(context.ModelState);
                return;
            }

            base.OnActionExecuted(context);
        }
    }
}
