using MattCanello.NewsFeed.RssReader.Domain.Extensions;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Parsers;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Responses;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Parsers
{
    public sealed class HtmlContentParser : IContentParser
    {
        private readonly static Regex FigureRegex = new Regex(@"<figure>(?:(?!<figure>|<\/figure>).)*<img[^>]*>(?:(?!<figure>|<\/figure>).)*<\/figure>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
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
                var xDoc = new XPathDocument(new StringReader(figureHtml));
                var nav = xDoc.CreateNavigator();
                var imgNode = nav.SelectSingleNode(".//img");

                if (imgNode is null)
                    return null;

                var alt = imgNode.GetAttribute("alt", "")?.ToNullWhenEmpty();
                var src = imgNode.GetAttribute("src", "")?.ToNullWhenEmpty();

                var figcaption = nav.SelectSingleNode(".//figcaption")?.Value?.ToNullWhenEmpty();

                return new HtmlFigure(new HtmlImg(src, alt), figcaption);
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
            public HtmlFigure() { }
            public HtmlFigure(HtmlImg? image, string? caption) 
            {
                Image = image;
                Caption = caption;
            }

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
            public HtmlImg() { }
            public HtmlImg(string? source, string? altText = null) 
            {
                Source = source;
                AltText = altText;
            }

            [XmlAttribute("alt")]
            public string? AltText { get; set; }

            [XmlAttribute("src")]
            public string? Source { get; set; }
        }
    }
}
