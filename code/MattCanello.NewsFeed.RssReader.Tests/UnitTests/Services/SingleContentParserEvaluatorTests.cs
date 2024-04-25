using MattCanello.NewsFeed.RssReader.Domain.Services;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Services
{
    public class SingleContentParserEvaluatorTests
    {
        [Fact]
        public void EvaluateParser_GivenNullItem_ShouldThrowException()
        {
            var evaluator = new SingleContentParserEvaluator(null!);

            var ex = Assert.Throws<ArgumentNullException>(() => evaluator.EvaluateParser(null!));

            Assert.NotNull(ex);
            Assert.Equal("item", ex.ParamName);
        }

        [Fact]
        public void EvaluateParser_GivenSyndicationItem_ShouldReturnExpectedParser()
        {
            var item = new SyndicationItem
            {
                Content = new TextSyndicationContent("")
            };

            var evaluator = new SingleContentParserEvaluator(MockedContentParser.Instance);

            var parser = evaluator.EvaluateParser(item);

            Assert.Equal(MockedContentParser.Instance, parser);
        }

        [Fact]
        public void EvaluateParser_GivenSyndicationItemWithNullContent_ShouldReturnExpectedParser()
        {
            var item = new SyndicationItem();

            var evaluator = new SingleContentParserEvaluator(MockedContentParser.Instance);

            var parser = evaluator.EvaluateParser(item);

            Assert.Null(parser);
        }
    }
}
