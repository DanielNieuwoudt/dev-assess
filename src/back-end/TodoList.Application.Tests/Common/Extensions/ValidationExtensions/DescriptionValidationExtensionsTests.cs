using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentValidation;
using TodoList.Application.Common.Extensions;

namespace TodoList.Application.Tests.Common.Extensions.ValidationExtensions
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class DescriptionValidationExtensionsTests
    {
        private readonly IdValidatorTestsValidator _validator = new();

        public class ValidationTarget
        {
            public string? Description { get; set; }
        }

        private class IdValidatorTestsValidator : AbstractValidator<ValidationTarget>
        {
            public IdValidatorTestsValidator()
            {
                RuleFor(x => x.Description)!
                    .ValidateDescription();
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", false)]
        [InlineData("Description", true)]
        public void Given_Id_When_IsValid_Then_ValidationResult(string? description, bool expectedValidationResult)
        {
            var subject = new ValidationTarget
            {
                Description = description!
            };
            _validator.Validate(subject)
                .IsValid
                .Should()
                .Be(expectedValidationResult);
        }
    }
}
