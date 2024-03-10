using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Processors;

namespace MattCanello.NewsFeed.SearchApi.Tests.Infrastructure.ElaticSearch.Processors
{
    public sealed class QueryStringProcessorTests
    {
        [Theory]
        [InlineData((string?)null)]
        [InlineData("")]
        public void Process_GivenEmptyQuery_ShouldReturnWildcard(string? query)
        {
            var processor = new QueryStringProcessor();

            var processedQuery = processor.Process(query);

            Assert.Equal("*", processedQuery);
        }

        [Theory]
        [InlineData("MATEUS")]
        [InlineData("Mateus")]
        [InlineData("MaTeUs")]
        [InlineData("mAtEuS")]
        [InlineData("mateus")]
        public void Process_GivenUpperCaseWords_ShouldResultInLowerCase(string? query)
        {
            var processoer = new QueryStringProcessor();

            var processedQuery = processoer.Process(query);

            Assert.Equal("*mateus*", processedQuery);
        }

        [Theory]
        [InlineData("Mancha +tinta (tecido) :and operação ordinária?")]
        public void Process_GivenInvalidQuery_ShouldResultInValidQuery(string query)
        {
            var processoer = new QueryStringProcessor();

            var processedQuery = processoer.Process(query);

            Assert.Equal("*mancha* *tinta* *tecido* *operação* *ordinária*", processedQuery);
        }

        [Theory]
        [InlineData((string?)null)]
        [InlineData("")]
        public void EnsureSingleLine_GivenNull_ShouldReturnStringEmpty(string? query)
        {
            var processedQuery = QueryStringProcessor.EnsureSingleLine(query);

            Assert.Equal(string.Empty, processedQuery);
        }

        [Theory]
        [InlineData("\rA\rB\rC\r")]
        [InlineData("\r\nA\r\nB\r\nC\r\n")]
        [InlineData("\nA\nB\nC\n")]
        public void EnsureSingleLine_GivenMultilineString_ShouldConcat(string query)
        {
            var processedQuery = QueryStringProcessor.EnsureSingleLine(query);

            Assert.Equal(" A B C ", processedQuery);
        }

        [Fact]
        public void StripUnsafeChars_GivenNull_ShouldReturnStringEmpty()
        {
            var processedQuery = QueryStringProcessor.StripUnsafeChars(null!);

            Assert.Equal(string.Empty, processedQuery);
        }

        [Theory]
        [InlineData("*a*a*")]
        [InlineData("?a?a?")]
        [InlineData(":a:a:")]
        [InlineData("'a'a'")]
        [InlineData("+a+a+")]
        [InlineData("-a-a-")]
        [InlineData("(a(a(")]
        [InlineData(")a)a)")]
        [InlineData("\"a\"a\"")]
        [InlineData(">a>a>")]
        [InlineData("<a<a<")]
        [InlineData("[a[a[")]
        [InlineData("]a]a]")]
        [InlineData("{a{a{")]
        [InlineData("}a}a}")]
        public void StripUnsafeChars_GivenUnsafeChar_ShouldNotBePresent(string query)
        {
            var processedQuery = QueryStringProcessor.StripUnsafeChars(query);

            Assert.Equal("aa", processedQuery);
        }

        [Fact]
        public void RemoveOperatorWords_GivenNull_ShouldReturnStringEmpty()
        {
            var processedQuery = QueryStringProcessor.RemoveOperatorWords(null!);

            Assert.Equal(string.Empty, processedQuery);
        }

        [Theory]
        [InlineData("or a or a or")]
        [InlineData("not a not a not")]
        [InlineData("and a and a and")]
        public void RemoveOperatorWords_GivenOperatorWord_ShouldNotBePresent(string query)
        {
            var processedQuery = QueryStringProcessor.RemoveOperatorWords(query);

            Assert.Equal("a  a", processedQuery);
        }

        [Theory]
        [InlineData("mortage")]     // meio
        [InlineData("knot")]        // fim
        [InlineData("android")]     // início
        public void RemoveOperatorWords_GivenOperatorWordAsPartOfTheWord_ShouldPreserveWord(string query)
        {
            var processedQuery = QueryStringProcessor.RemoveOperatorWords(query);

            Assert.Equal(query, processedQuery);
        }

        [Theory]
        [InlineData((string?)null)]
        [InlineData("")]
        public void RemoveMultipleWhiteSpaces_GivenEmptyQuery_ShuoldReturnStringEmpty(string? query)
        {
            var processedQuery = QueryStringProcessor.RemoveMultipleWhiteSpaces(query!);

            Assert.Equal(string.Empty, processedQuery);
        }

        [Theory]
        [InlineData("  a   a    ")]
        public void RemoveMultipleWhiteSpaces_GivenAQueryWithManyWhiteSpaces_ShouldKeepOnlyOne(string query)
        {
            var processedQuery = QueryStringProcessor.RemoveMultipleWhiteSpaces(query);

            Assert.Equal(" a a ", processedQuery);
        }

        [Theory]
        [InlineData("a a")]
        public void SurroundEachWordWithWildcard_GivenTwoWords_ShouldSurrountWithWildcard(string query)
        {
            var processedQuery = QueryStringProcessor.SurroundEachWordWithWildcard(query);

            Assert.Equal("*a* *a*", processedQuery);
        }
    }
}
