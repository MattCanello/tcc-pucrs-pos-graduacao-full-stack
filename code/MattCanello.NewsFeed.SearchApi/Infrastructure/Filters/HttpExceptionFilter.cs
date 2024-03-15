using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Filters
{
    [ExcludeFromCodeCoverage]
    public sealed class HttpExceptionFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger, IUrlHelperFactory urlHelperFactory)
        {
            _logger = logger;
            _urlHelperFactory = urlHelperFactory;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is EntryAlreadyIndexedException alreadyIndexedException)
            {
                context.Exception = null;
                context.ModelState.AddModelError("Entry", alreadyIndexedException.Message);

                var document = alreadyIndexedException.Document;
                var location = BuildGetDocumentByIdUrl(context, document?.FeedId, document?.Id);

                context.Result = new ConflictObjectResult(context.ModelState);
                context.HttpContext.Response.Headers.Add(HeaderNames.Location, location);
                return;
            }

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

        private string BuildGetDocumentByIdUrl(ActionContext actionContext, string? feedId, string? id)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

            var path = urlHelper.Action("GetById", "Document", new { feedId, id });

            var uriBuilder = new UriBuilder()
            {
                Host = actionContext.HttpContext.Request.Host.Host,
                Path = path,
                Scheme = actionContext.HttpContext.Request.Scheme
            };

            if (actionContext.HttpContext.Request.Host.Port != null)
                uriBuilder.Port = actionContext.HttpContext.Request.Host.Port.Value;

            return uriBuilder.ToString();
        }
    }
}
