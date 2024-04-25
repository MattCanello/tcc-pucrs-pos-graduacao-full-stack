using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Parsers;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Responses;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Parsers
{
    public sealed class HtmlContentParser : IContentParser
    {
        private readonly static Regex FigureRegex = new Regex(@"<figure>.*<\/figure>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private readonly ILogger<HtmlContentParser> _logger;

        public HtmlContentParser(ILogger<HtmlContentParser> logger)
        {
            _logger = logger;
        }

        public ParsedContent? TryParse(SyndicationContent content)
        {
            ArgumentNullException.ThrowIfNull(content);

            if (content is not TextSyndicationContent textSyndicationContent)
                return null;

            if (!"html".Equals(content.Type))
                return null;

            var html = textSyndicationContent.Text;
            if (string.IsNullOrEmpty(html))
                return null;

            html = WebUtility
                .HtmlDecode(html)
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("\t", "");

            var figureMatch = FigureRegex.Match(html);

            Thumbnail? thumb = null;

            if (figureMatch.Success)
            {
                thumb = TryExtractThumbnail(figureMatch.Groups[0].Value);
                html = html.Replace(figureMatch.Groups[0].Value, "");
            }

            return new ParsedContent(html, thumb);
        }

        public Thumbnail? TryExtractThumbnail(string figureHtml)
        {
            ArgumentNullException.ThrowIfNull(figureHtml);

            var htmlFigure = TryParseHtmlFigure(figureHtml);

            return htmlFigure?.ToThumbnail();
        }

        private HtmlFigure? TryParseHtmlFigure(string figureHtml)
        {
            ArgumentNullException.ThrowIfNull(figureHtml);

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(HtmlFigure));
                var htmlFigure = xmlSerializer.Deserialize(new StringReader(figureHtml)) as HtmlFigure;
                return htmlFigure;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to deserializer the figure HTML.");
                return null;
            }
        }

        [Serializable]
        [XmlRoot("figure")]
        public sealed record HtmlFigure
        {
            [XmlElement("img")]
            public HtmlImg? Image { get; set; }

            [XmlElement("figcaption")]
            public string? Caption { get; set; }

            public Thumbnail? ToThumbnail()
            {
                if (Image is null || string.IsNullOrEmpty(Image.Source))
                    return null;

                return new Thumbnail()
                {
                    Url = Image.Source,
                    Credit = Image.AltText ?? Caption
                };
            }
        }

        [Serializable]
        [XmlRoot("img")]
        public sealed record HtmlImg
        {
            [XmlAttribute("alt")]
            public string? AltText { get; set; }

            [XmlAttribute("src")]
            public string? Source { get; set; }
        }
    }
}
