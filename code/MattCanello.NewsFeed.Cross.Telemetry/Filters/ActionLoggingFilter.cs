using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;

namespace MattCanello.NewsFeed.Cross.Telemetry.Filters
{
    public sealed class ActionLoggingFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using var state = _logger.BeginScope(context);

            WriteRequestLog(context.HttpContext);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await next();
            }
            finally
            {
                stopwatch.Stop();

                WriteResponseLog(context.HttpContext, stopwatch.ElapsedMilliseconds);
            }
        }

        private void WriteRequestLog(HttpContext httpContext)
        {
            if (httpContext?.Request is null)
                return;

            _logger.LogInformation("Begin request {Method} {Path}",
               httpContext.Request.Method,
               httpContext.Request.Path);
        }

        private void WriteResponseLog(HttpContext httpContext, long elapsedMilliseconds)
        {
            if (httpContext?.Response is null)
                return;

            var logLevel = httpContext.Response.StatusCode >= 200 && httpContext.Response.StatusCode <= 299
                ? LogLevel.Information
                : LogLevel.Error;

            var httpStatusCode = (HttpStatusCode)httpContext.Response.StatusCode;

            _logger.Log(logLevel, "Request ended {Method} {Path} with {StatusCode}/{httpStatusCode} - {elapsedMilliseconds} ms.",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Response.StatusCode,
                httpStatusCode,
                elapsedMilliseconds);
        }
    }
}
