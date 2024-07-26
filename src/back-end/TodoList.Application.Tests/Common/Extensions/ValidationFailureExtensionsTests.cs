using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentValidation.Results;
using TodoList.Application.Common.Extensions;

namespace TodoList.Application.Tests.Common.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class ValidationFailureExtensionsTests
    {
        [Fact]
        public void Given_ValidationFailures_When_IsNull_Then_ReturnEmptyDictionary()
        {
            List<ValidationFailure>? validationFailures = null;

            var result = validationFailures.ToErrorDictionary();
            
            result
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Given_ValidationFailures_When_IsEmpty_Then_ReturnEmptyDictionary()
        {
            var validationFailures = new List<ValidationFailure>();

            var result = validationFailures.ToErrorDictionary();
            
            result
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Given_ValidationFailures_When_HasFailures_Then_ReturnErrorDictionary()
        {
            var property = "property";
            var value = "value";

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(property, value),
            };

            var result = validationFailures.ToErrorDictionary();

            result
                .Should()
                .HaveCount(1)
                .And
                .ContainKey(property)
                .WhoseValue
                .Should()
                .BeEquivalentTo(new[] { value });
        }
    }
}
