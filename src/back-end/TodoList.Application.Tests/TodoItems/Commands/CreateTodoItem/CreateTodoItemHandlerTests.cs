using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Application.Common.Exceptions;
using TodoList.Application.Contracts;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Tests.TodoItems.Commands.CreateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class CreateTodoItemHandlerTests
    {
        private readonly Mock<ITodoItemsRepository> _repositoryMock = new ();
        private readonly NullLogger<CreateTodoItemHandler> _nullLogger = new ();

        [Fact]
        public void Given_NullRepository_When_CreateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var act = () => new CreateTodoItemHandler(null!, _nullLogger);

            act
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_CreateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var act = () => new CreateTodoItemHandler(_repositoryMock.Object, null!);

            act
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_CreateTodoItemCommand_When_NoDuplicateTodoItemFound_Then_ReturnsCreateTodoItemResult()
        {
            var id = Guid.NewGuid();

            var command = new CreateTodoItemCommand(id, "Description", false);
            var todoItem = new TodoItem(new TodoItemId(id), "Description", false, DateTimeOffset.Now, DateTimeOffset.Now);

            _repositoryMock
                .Setup(r => r.FindDuplicateTodoItemAsync(It.IsAny<Expression<Func<TodoItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _repositoryMock
                .Setup(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItem);

            var handler = new CreateTodoItemHandler(_repositoryMock.Object, _nullLogger);

            var result = await handler
                .Handle(command, CancellationToken.None);

            result.TodoItem
                .Should()
                .Be(todoItem);

            _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Given_CreateTodoItemCommand_When_DuplicateTodoItemFoundByDescription_Then_ThrowsTodoItemDuplicateException()
        {
            var id = Guid.NewGuid();
            var command = new CreateTodoItemCommand(id, "Description", false);

            _repositoryMock
                .Setup(r => r.FindDuplicateTodoItemAsync(ti => ti.Description == command.Description && ti.IsCompleted == false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new CreateTodoItemHandler(_repositoryMock.Object, _nullLogger);

            var action = async () => await handler.Handle(command, CancellationToken.None);

            await action
                .Should()
                .ThrowAsync<TodoItemDuplicateException>();

            _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Given_CreateTodoItemCommand_When_DuplicateTodoItemFoundById_Then_ThrowsTodoItemDuplicateException()
        {
            var id = Guid.NewGuid();
            var command = new CreateTodoItemCommand(id, "Description", false);

            _repositoryMock
                .Setup(r => r.FindDuplicateTodoItemAsync(
                    It.Is<Expression<Func<TodoItem, bool>>>(expr =>
                        MatchesExpression(ti => ti.Id == new TodoItemId(id) && ti.IsCompleted == false, new TodoItemId(id))),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new CreateTodoItemHandler(_repositoryMock.Object, _nullLogger);

            var action = async () => await handler.Handle(command, CancellationToken.None);

            await action
                .Should()
                .ThrowAsync<TodoItemDuplicateException>();

            _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private bool MatchesExpression(Expression<Func<TodoItem, bool>> expr, TodoItemId expectedId)
        {
            var compiledExpr = expr.Compile();
            return compiledExpr(new TodoItem(expectedId, "Description", false, DateTimeOffset.Now, DateTimeOffset.Now));
        }
    }
}
