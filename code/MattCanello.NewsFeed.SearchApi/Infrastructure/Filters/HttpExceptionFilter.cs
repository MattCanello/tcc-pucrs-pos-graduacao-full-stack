using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Filters
{
    [ExcludeFromCodeCoverage]
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
                _logger.LogCritical(indexException, "Unable to index a document on '{indexName}'", indexException.IndexName);

            if (context.Exception is DocumentNotFoundException)
            {
                context.Exception = null;
                context.Result = new NotFoundResult();
                return;
            }

            base.OnActionExecuted(context);
        }
    }
}
