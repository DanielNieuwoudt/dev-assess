using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentValidation;
using TodoList.Application.TodoItems.Extensions;

namespace TodoList.Application.Tests.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class IdValidationExtensionsTests
    {
        private readonly IdValidatorTestsValidator _validator = new();

        public class ValidationTarget
        {
            public Guid Id { get; set; }
        }

        private class IdValidatorTestsValidator : AbstractValidator<ValidationTarget>
        {
            public IdValidatorTestsValidator()
            {
                RuleFor(x => x.Id).ValidateId();
            }
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", false)]
        [InlineData("a662c676-b165-4fd3-9683-92d4f4460617", true)]
        public void Given_Id_When_IsValid_Then_ValidationResult(Guid id, bool expectedValidationResult)
        {
            var subject = new ValidationTarget
            {
                Id = id
            };
            _validator.Validate(subject)
                .IsValid
                .Should()
                .Be(expectedValidationResult);
        }
    }
}
