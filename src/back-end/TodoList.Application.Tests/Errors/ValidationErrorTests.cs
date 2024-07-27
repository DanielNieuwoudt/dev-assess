using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TodoList.Application.Common.Enumerations;
using TodoList.Application.Common.Errors;

namespace TodoList.Application.Tests.Errors
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class ValidationErrorTests
    {
        [Fact]
        public void Given_ReasonPropertyMessage_When_Initialised_Then_SetsErrorsAndReason()
        {
            var property = "property";
            var message = "message";

            var validationError = new ValidationError(property, message);

            validationError.Reason
                .Should()
                .Be(ErrorReason.Validation);

            validationError
                .Errors.Should()
                .ContainKey(property)
                .WhoseValue
                .Should()
                .BeEquivalentTo(message);
        }

        [Fact]
        public void Given_ReasonErrors_When_Initialised_Then_SetsErrorsAndReason()
        {
            var errors = new Dictionary<string, string[]>
            {
                { "property", new[] { "message" } }
            };

            var validationError = new ValidationError(errors);

            validationError.Reason
                .Should()
                .Be(ErrorReason.Validation);

            validationError
                .Errors
                .Should()
                .BeEquivalentTo(errors);
        }
    }
}
