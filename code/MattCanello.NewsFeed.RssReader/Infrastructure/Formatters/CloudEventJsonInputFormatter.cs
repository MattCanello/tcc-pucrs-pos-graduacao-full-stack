using CloudNative.CloudEvents;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Formatters
{
    public class CloudEventJsonInputFormatter : TextInputFormatter
    {
        private readonly CloudEventFormatter _formatter;

        public CloudEventJsonInputFormatter(CloudEventFormatter formatter)
        {
            ArgumentNullException.ThrowIfNull(formatter);

            _formatter = formatter;
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/cloudevents+json"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(context);
            var request = context.HttpContext.Request;

            try
            {
                var contentType = string.IsNullOrEmpty(request.ContentType) ? null : new ContentType(request.ContentType);
                var cloudEvent = await _formatter.DecodeStructuredModeMessageAsync(request.Body, contentType, null);
                return await InputFormatterResult.SuccessAsync(cloudEvent);
            }
            catch (Exception)
            {
                return await InputFormatterResult.FailureAsync();
            }
        }

        protected override bool CanReadType(Type type)
             => type == typeof(CloudEvent) && base.CanReadType(type);
    }
}
