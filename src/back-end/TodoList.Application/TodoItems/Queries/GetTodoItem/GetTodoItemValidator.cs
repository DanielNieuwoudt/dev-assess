using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using TodoList.Application.TodoItems.Extensions;

namespace TodoList.Application.TodoItems.Queries.GetTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Tested as validation extensions.")]
    public sealed class GetTodoItemValidator : AbstractValidator<GetTodoItemQuery>  
    {
        public GetTodoItemValidator()
        {
            RuleFor(ti => ti.Id)
                .ValidateId();
        }
    }
}
