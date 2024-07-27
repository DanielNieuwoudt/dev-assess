using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TodoList.Application.Common.Enumerations;
using TodoList.Application.Common.Errors;

namespace TodoList.Application.Tests.Errors
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class DuplicateErrorTests
    {
        [Fact]
        public void Given_ReasonPropertyMessage_When_Initialised_Then_SetsErrorsAndReason()
        {
            var property = "property";
            var message = "message";

            var duplicateError = new DuplicateError(property, message);

            duplicateError.Reason
                .Should()
                .Be(ErrorReason.Duplicate);

            duplicateError
                .Errors.Should()
                .ContainKey(property)
                .WhoseValue
                .Should()
                .BeEquivalentTo(message);
        }
    }
}
