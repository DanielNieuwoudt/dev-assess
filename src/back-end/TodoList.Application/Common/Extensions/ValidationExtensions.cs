using FluentValidation;

namespace TodoList.Application.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, Guid> ValidateId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty();
        }

        public static IRuleBuilderOptions<T, string> ValidateDescription<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
