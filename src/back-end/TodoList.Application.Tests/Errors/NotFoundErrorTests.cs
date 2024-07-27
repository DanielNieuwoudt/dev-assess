using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TodoList.Application.Common.Enumerations;
using TodoList.Application.Common.Errors;

namespace TodoList.Application.Tests.Errors
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class NotFoundErrorTests
    {
        [Fact]
        public void Given_ReasonPropertyMessage_When_Initialised_Then_SetsErrorsAndReason()
        {
            var property = "property";
            var message = "message";

            var notFoundError = new NotFoundError(property, message);

            notFoundError.Reason
                .Should()
                .Be(ErrorReason.NotFound);

            notFoundError
                .Errors.Should()
                .ContainKey(property)
                .WhoseValue
                .Should()
                .BeEquivalentTo(message);
        }
    }
}
