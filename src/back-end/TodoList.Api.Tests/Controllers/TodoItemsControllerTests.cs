﻿using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Api.Controllers;
using TodoList.Api.Mapping;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.TodoItems.GetTodoItems;
using TodoList.Application.TodoItems.Queries.GetTodoItem;
using TodoList.Domain.TodoItems.ValueObjects;
using Xunit;

namespace TodoList.Api.Tests.Controllers
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TodoItemsControllerTests
    {
        private readonly Mock<ISender> _senderMock = new();
        private readonly NullLogger<TodoItemsController> _nullLogger = new();
        private readonly IMapper _mapper;

        public TodoItemsControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TodoItemMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void Given_NullSender_When_TodoItemsControllerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsController(GetTodoContext(), _mapper, null!, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_TodoItemsControllerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsController(GetTodoContext(), _mapper, _senderMock.Object, null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullMapper_When_TodoItemsControllerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsController(GetTodoContext(), null!, _senderMock.Object, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullContext_When_TodoItemsControllerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsController(null!, _mapper, _senderMock.Object, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }
        
        [Fact]
        public async Task Given_GetTodoItem_When_SendGetTodoItemQuery_Then_ReturnsContractItem()
        {
            var domainTodoItem = new Domain.TodoItems.TodoItem(new TodoItemId(Guid.NewGuid()), "description", false,
                DateTimeOffset.Now, DateTimeOffset.Now);

            _senderMock
                .Setup(x => x.Send(It.IsAny<GetTodoItemQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTodoItemResult(domainTodoItem));

            var todoItemController = new TodoItemsController(GetTodoContext(), _mapper, _senderMock.Object, _nullLogger);

            var result = await todoItemController
                .GetTodoItem(Guid.NewGuid(), CancellationToken.None);

            var okResult = result as OkObjectResult;
                
            okResult
                .Should()
                .NotBeNull();

            okResult!
                .Value
                .Should()
                .BeEquivalentTo(_mapper.Map<Generated.TodoItem>(domainTodoItem));
        }

        [Fact]
        public async Task Given_GetTodoItems_When_SendGetTodoItemsQuery_Then_ReturnsContractItems()
        {
            _senderMock
                .Setup(x => x.Send(It.IsAny<GetTodoItemsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTodoItemsResult(new List<Domain.TodoItems.TodoItem>()));

            var todoItemController = new TodoItemsController(GetTodoContext(), _mapper, _senderMock.Object, _nullLogger);

            var result = await todoItemController
                .GetTodoItems(CancellationToken.None);

            var okResult = result as OkObjectResult;
            
            okResult
                .Should()
                .NotBeNull();

            okResult!
                .Value
                .Should()
                .BeEquivalentTo(new List<Generated.TodoItem>());
        }

        [Fact]
        public async Task Given_PostTodoItem_When_SendCreateTodoItemCommand_Then_ReturnsTodoItemFromContract()
        {
            var domainTodoItem = new Domain.TodoItems.TodoItem(new TodoItemId(Guid.NewGuid()), "description", false, DateTimeOffset.Now, DateTimeOffset.Now);

            _senderMock
                .Setup(x => x.Send(It.IsAny<CreateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateTodoItemResult(domainTodoItem));

            var todoItemController = new TodoItemsController(GetTodoContext(), _mapper, _senderMock.Object, _nullLogger);

            var result = await todoItemController
                .PostTodoItem(new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Description",
                    IsCompleted = false
                });

            var createdResult = result as CreatedAtActionResult;

            createdResult
                .Should()
                .NotBeNull();

            createdResult!.Value.Should()
                .BeEquivalentTo(_mapper.Map<Generated.TodoItem>(domainTodoItem));
        }

        private static TodoContext GetTodoContext()
        {
            var serviceCollection = new ServiceCollection()
                .AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoItemsDB"));
            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider.GetRequiredService<TodoContext>();
        }
    }
}
