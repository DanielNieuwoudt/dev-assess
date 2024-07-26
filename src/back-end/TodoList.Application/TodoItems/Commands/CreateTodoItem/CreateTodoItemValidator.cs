using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using TodoList.Application.Common.Extensions;

namespace TodoList.Application.TodoItems.Commands.CreateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Tested as validation extensions.")]
    public sealed class CreateTodoItemValidator : AbstractValidator<CreateTodoItemCommand>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(ti => ti.Id)
                .ValidateId();
            RuleFor(ti => ti.Description)
                .ValidateDescription();
        }
    }
}
