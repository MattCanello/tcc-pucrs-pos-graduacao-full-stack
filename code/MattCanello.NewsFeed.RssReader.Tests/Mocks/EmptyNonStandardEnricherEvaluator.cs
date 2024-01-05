using MattCanello.NewsFeed.RssReader.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class EmptyNonStandardEnricherEvaluator : INonStandardEnricherEvaluator
    {
        public static readonly EmptyNonStandardEnricherEvaluator Instance = new EmptyNonStandardEnricherEvaluator();

        public INonStandardRssEnricher? Evaluate(string @namespace)
        {
            return null;
        }
    }
}
