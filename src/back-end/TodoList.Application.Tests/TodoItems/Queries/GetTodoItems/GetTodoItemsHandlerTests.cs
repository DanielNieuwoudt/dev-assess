﻿using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics.CodeAnalysis;
using Moq;
using TodoList.Application.Contracts;
using TodoList.Application.TodoItems.GetTodoItems;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Tests.TodoItems.GetTodoItems
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class GetTodoItemsHandlerTests
    {
        
        private readonly Mock<ITodoItemsRepository> _repositoryMock = new();
        private readonly NullLogger<GetTodoItemsHandler> _nullLogger = new();

        [Fact]
        public void Given_Repository_When_HandlerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new GetTodoItemsHandler(null!, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_HandlerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new GetTodoItemsHandler(_repositoryMock.Object, null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_Request_When_Handle_Then_ReturnsTodoItems()
        {
            var todoItems = new List<TodoItem>
            {
                new (new TodoItemId(Guid.NewGuid()), "Test 1", false, DateTimeOffset.Now, DateTimeOffset.Now),
                new (new TodoItemId(Guid.NewGuid()), "Test 1", false, DateTimeOffset.Now, DateTimeOffset.Now)
            };

            _repositoryMock
                .Setup(x => x.GetTodoItemsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItems);

            var handler = new GetTodoItemsHandler(_repositoryMock.Object, new NullLogger<GetTodoItemsHandler>());

            var result = await handler.Handle(new GetTodoItemsQuery(), CancellationToken.None);

            result
                .TodoItems
                .Should()
                .BeEquivalentTo(todoItems);
        }
    }
}
