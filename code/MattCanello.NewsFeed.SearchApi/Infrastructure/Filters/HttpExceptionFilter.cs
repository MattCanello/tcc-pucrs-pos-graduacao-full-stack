using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Filters
{
    public sealed class HttpExceptionFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is IndexException indexException)
                _logger.LogCritical(indexException, "Unable to index an entry");

            base.OnActionExecuted(context);
        }
    }
}
