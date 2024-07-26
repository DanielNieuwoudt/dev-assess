using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentValidation;
using MediatR;
using TodoList.Application.Common.Behaviours;
using TodoList.Application.Common.Errors;
using TodoList.Application.TodoItems;

namespace TodoList.Application.Tests.Common.Behaviours
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TestBehaviour
    {
        public class TestRequest(Guid id) : IRequest<TodoItemResult<ApplicationError, TestResponse>>
        {
            public Guid Id = id;
        }

        public class TestResponse
        {
        }

        public class TestBehaviourValidator : AbstractValidator<TestRequest>
        {
            public TestBehaviourValidator()
            {
                RuleFor(p => p.Id)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<TestRequest, TodoItemResult<ApplicationError, TestResponse>>
        {
            public async Task<TodoItemResult<ApplicationError, TestResponse>> Handle(TestRequest testRequest, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new TestResponse());
            }
        }
    }

    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class ValidationBehaviorTests
    {
        [Theory(Skip = "Need to resolve activator for validation behaviour.")]
        [InlineData("00000000-0000-0000-0000-000000000000", true)]
        [InlineData("2f5adcb0-e180-4839-a64a-c7f3fb3b449f", false)]
        public async Task GivenRequest_When_Handle_Then_ShouldThrowValidationExceptionIfValidationFails(string id,
            bool shouldFail)
        {
            var validators = new List<TestBehaviour.TestBehaviourValidator>
            {
                new()
            };

            var validationBehavior =
                new ValidationBehavior<TestBehaviour.TestRequest, TodoItemResult<ApplicationError, TestBehaviour.TestResponse>>(
                    validators);

            Func<Task<TodoItemResult<ApplicationError, TestBehaviour.TestResponse>>> response = async () =>
            {
                return await validationBehavior
                    .Handle(new TestBehaviour.TestRequest(Guid.Parse(id)), Next,
                        CancellationToken.None);

                Task<TodoItemResult<ApplicationError, TestBehaviour.TestResponse>> Next()
                {
                    return Task.FromResult(
                        new TodoItemResult<ApplicationError, TestBehaviour.TestResponse>(new TestBehaviour.TestResponse()));
                }
            };

            var result = await response.Invoke();

            if (shouldFail)
            {
                result.Error.Should().BeOfType<ValidationError>();
                result.Error.Should().NotBeNull();
                result.Value.Should().BeNull();
            }
            else
            {
                result.Value.Should().BeOfType<TestBehaviour.TestResponse>();
                result.Value.Should().NotBeNull();
                result.Error.Should().BeNull();
            }
        }
    }
}
