using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TodoList.Api.Common.Constants;
using Xunit;

namespace TodoList.Api.Tests.Common.Constants
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class ResponseTypesTests
    {
        [Theory]
        [InlineData(ResponseTypes.BadRequest, "https://tools.ietf.org/html/rfc7231#section-6.5.1")]
        [InlineData(ResponseTypes.InternalServerError, "https://tools.ietf.org/html/rfc7231#section-6.6.1")]
        [InlineData(ResponseTypes.NotFound, "https://tools.ietf.org/html/rfc7231#section-6.5.4")]
        public void Given_ResponseTypeConstants_When_Used_Then_ReturnsExpectedValue(string constantValue, string expectedValue)
        {
            constantValue
                .Should()
                .Be(expectedValue);
        }
    }
}
