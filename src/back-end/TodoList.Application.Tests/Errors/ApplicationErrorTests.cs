using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TodoList.Application.Common.Enumerations;
using TodoList.Application.Common.Errors;

namespace TodoList.Application.Tests.Errors
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class ApplicationErrorTests
    {
        [Fact]
        public void Given_ReasonPropertyMessage_When_Initialised_Then_SetsErrorsAndReason()
        {
            var property = "property";
            var message = "message";

            var applicationError = new TestApplicationError(ErrorReason.NotFound, property, message);

            applicationError.Reason
                .Should()
                .Be(ErrorReason.NotFound);

            applicationError
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

            var applicationError = new TestApplicationError(ErrorReason.NotFound, errors);

            applicationError.Reason
                .Should()
                .Be(ErrorReason.NotFound);

            applicationError
                .Errors
                .Should()
                .BeEquivalentTo(errors);
        }
    }

    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public sealed class TestApplicationError : ApplicationError
    {
        public TestApplicationError(ErrorReason reason, string property, string message) : base(reason, property, message)
        {
        }

        public TestApplicationError(ErrorReason reason, IDictionary<string, string[]> errors) : base(reason, errors)
        {
        }
    }
}
