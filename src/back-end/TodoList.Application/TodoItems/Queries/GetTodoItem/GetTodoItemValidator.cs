using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using TodoList.Application.Common.Extensions;

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
