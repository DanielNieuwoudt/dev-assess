using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using TodoList.Application.Extensions;

namespace TodoList.Application.TodoItems.Commands.UpdateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Tested as validation extensions.")]
    public sealed class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemCommand>
    {
        public UpdateTodoItemValidator()
        {
            RuleFor(ti => ti.RouteId)
                .ValidateId();
            RuleFor(ti => ti.Id)
                .ValidateId();
            RuleFor(ti => ti.Description)
                .ValidateDescription();
        }
    }
}
