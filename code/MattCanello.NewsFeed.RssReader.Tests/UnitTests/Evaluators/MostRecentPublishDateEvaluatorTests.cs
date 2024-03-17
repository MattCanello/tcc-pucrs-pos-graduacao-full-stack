using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Evaluators;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Evaluators
{
    public class MostRecentPublishDateEvaluatorTests
    {
        [Theory, AutoData]
        public void Evaluate_WhenCurrentIsNull_ShouldReturnOther(DateTimeOffset other)
        {
            var evaluator = new MostRecentPublishDateEvaluator();
            
            var result = evaluator.Evaluate(null, other);

            Assert.Equal(other, result);
        }

        [Theory, AutoData]
        public void Evaluate_WhenCurrentIsGreaterThanOther_ShouldReturnCurrent(DateTimeOffset current)
        {
            var other = current.AddDays(-1);

            var evaluator = new MostRecentPublishDateEvaluator();

            var result = evaluator.Evaluate(current, other);

            Assert.Equal(current, result);
        }

        [Theory, AutoData]
        public void Evaluate_WhenOtherIsGreaterThanCurrent_ShouldReturnOther(DateTimeOffset other)
        {
            var current = other.AddDays(-1);

            var evaluator = new MostRecentPublishDateEvaluator();

            var result = evaluator.Evaluate(current, other);

            Assert.Equal(other, result);
        }
    }
}
