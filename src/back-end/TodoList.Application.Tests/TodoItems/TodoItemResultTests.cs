using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TodoList.Application.Common.Enumerations;
using TodoList.Application.Common.Errors;
using TodoList.Application.TodoItems;

namespace TodoList.Application.Tests.TodoItems
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TodoItemResultTests
    {
        [Fact]
        public void Given_Value_When_Initialised_Then_ReturnsResultWithValue()
        {
            var value = "Value";

            var result = new TodoItemResult<ApplicationError, string>(value);

            result.IsError
                .Should().BeFalse();
            
            result.Error
                .Should()
                .BeNull();

            result.Value
                .Should()
                .NotBeNull();
            
            result.Value
                .Should()
                .BeOfType<string>();

            result.Value
                .Should()
                .Be(value);
        }

        [Fact]
        public void Given_Error_When_Initialised_Then_ReturnsResultWithError()
        {
            var result = new TodoItemResult<ApplicationError, string>(new TestError(new Dictionary<string, string[]>()));

            result.IsError
                .Should()
                .BeTrue();

            result.Error
                .Should()
                .NotBeNull();

            result.Error
                .Should()
                .BeOfType<TestError>();
        }

        [Fact]
        public void Given_Value_When_ImplicitlyConverted_Then_ReturnsResultWithValue()
        {
            var value = "Value";

            TodoItemResult<ApplicationError, string> result = value;

            result.IsError
                .Should()
                .BeFalse();

            result.Error
                .Should()
                .BeNull();

            result.Value
                .Should()
                .NotBeNull();

            result.Value
                .Should()
                .BeOfType<string>();

            result.Value
                .Should()
                .Be(value);
        }

        [Fact]
        public void Given_Error_When_ImplicitlyConverted_Then_ReturnsResultWithError()
        {
            var error = new TestError(new Dictionary<string, string[]>());

            TodoItemResult<ApplicationError, string> result = error;

            result.IsError
                .Should()
                .BeTrue();

            result.Error
                .Should()
                .NotBeNull();

            result.Error
                .Should()
                .BeOfType<TestError>();
        }
    }


    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TestError(IDictionary<string, string[]> errors) : ApplicationError(ErrorReason.Duplicate, errors);
}
