using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Api.Common.Helpers;
using TodoList.Api.Controllers;
using TodoList.Api.Generated;
using TodoList.Api.Common.Mapping;
using TodoList.Application.Common.Errors;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.TodoItems.Queries.GetTodoItem;
using TodoList.Application.TodoItems.Queries.GetTodoItems;
using TodoList.Domain.TodoItems.ValueObjects;
using Xunit;

namespace TodoList.Api.Tests.Controllers
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TodoItemsControllerTests
    {
        private readonly TodoItemsController _todoItemController;
        private readonly Mock<IErrorHelper> _errorHelperMock = new ();
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

            _errorHelperMock.Setup(x => x.ValidationErrorResult(It.IsAny<ValidationError>()))
                .Returns(new BadRequestObjectResult(new BadRequest()));

            _errorHelperMock.Setup(x => x.NotFoundErrorResult(It.IsAny<NotFoundError>()))
                .Returns(new NotFoundObjectResult(new NotFound()));

            _errorHelperMock.Setup(x => x.DuplicateErrorResult(It.IsAny<DuplicateError>()))
                .Returns(new BadRequestObjectResult(new BadRequest()));

            _errorHelperMock.Setup(x => x.IdMismatchValidationError())
                .Returns(new BadRequestObjectResult(new BadRequest()));

            _todoItemController = new TodoItemsController(_errorHelperMock.Object, _mapper, _senderMock.Object, _nullLogger);
        }

        [Fact]
        public void Given_NullErrorHelper_When_TodoItemsControllerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsController(null!,_mapper, _senderMock.Object, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullMapper_When_TodoItemsControllerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsController(_errorHelperMock.Object,null!, _senderMock.Object, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void Given_NullSender_When_TodoItemsControllerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsController(_errorHelperMock.Object, _mapper, null!, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void Given_NullLogger_When_TodoItemsControllerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsController(_errorHelperMock.Object, _mapper, _senderMock.Object, null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_GetTodoItem_When_SendGetTodoItemQueryReturnsItem_Then_ReturnsOkObjectResultWithItem()
        {
            var id = Guid.NewGuid();
            var domainTodoItem = new Domain.TodoItems.TodoItem(new TodoItemId(id), "description", false,
                DateTimeOffset.Now, DateTimeOffset.Now);

            _senderMock
                .Setup(x => x.Send(It.IsAny<GetTodoItemQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTodoItemResponse(domainTodoItem));

            var result = await _todoItemController
                .GetTodoItem(id, CancellationToken.None);

            result.Result
                .Should()
                .NotBeNull();
            
            result.Result
                .Should()
                .BeOfType<OkObjectResult>();

            var okObjectResult = result
                .Result as OkObjectResult;

            okObjectResult!.Value
                .Should()
                .BeEquivalentTo(_mapper.Map<TodoItem>(domainTodoItem));
        }

        [Fact]
        public async Task Given_GetTodoItem_When_SendGetTodoItemQueryReturnsValidationError_Then_ReturnsBadRequestObjectResultWithErrorResponse()
        {
            _senderMock
                .Setup(x => x.Send(It.IsAny<GetTodoItemQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationError(new Dictionary<string, string[]>()));

            var result = await _todoItemController
                .GetTodoItem(Guid.Empty, CancellationToken.None);

            result.Result
                .Should()
                .NotBeNull();
            
            result.Result
                .Should()
                .BeOfType<BadRequestObjectResult>();

            var badRequestObjectResult = result
                .Result as BadRequestObjectResult;

            badRequestObjectResult!.Value
                .Should()
                .BeEquivalentTo(new BadRequest());
        }

        [Fact]
        public async Task Given_GetTodoItem_When_SendGetTodoItemQueryReturnsNotFoundError_Then_ReturnsNotFoundObjectResultWithErrorResponse()
        {
            _senderMock
                .Setup(x => x.Send(It.IsAny<GetTodoItemQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotFoundError(new Dictionary<string, string[]>()));

            var result = await _todoItemController
                .GetTodoItem(Guid.Empty, CancellationToken.None);

            result.Result
                .Should()
                .NotBeNull();
            
            result.Result
                .Should()
                .BeOfType<NotFoundObjectResult>();

            var badRequestObjectResult = result
                .Result as NotFoundObjectResult;

            badRequestObjectResult!.Value
                .Should()
                .BeEquivalentTo(new NotFound());
        }

        [Fact]
        public async Task Given_GetTodoItems_When_SendGetTodoItemsQueryReturnsItems_Then_ReturnsOkObjectResultWithItems()
        {
            _senderMock
                .Setup(x => x.Send(It.IsAny<GetTodoItemsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTodoItemsResponse(new List<Domain.TodoItems.TodoItem>()));

            var result = await _todoItemController
                .GetTodoItems(CancellationToken.None);

            result
                .Should()
                .NotBeNull();

            result.Result
                .Should()
                .BeOfType<OkObjectResult>();

            var okObjectResult = result
                .Result as OkObjectResult;

            okObjectResult!
                .Value
                .Should()
                .BeEquivalentTo(new List<TodoItem>());
        }

        [Fact]
        public async Task Given_PostTodoItem_When_SendCreateTodoItemCommandReturnsItem_Then_ReturnsCreatedAtActionResultWithItem()
        {
            var domainTodoItem = new Domain.TodoItems.TodoItem(new TodoItemId(Guid.NewGuid()), "description", false, DateTimeOffset.Now, DateTimeOffset.Now);

            _senderMock
                .Setup(x => x.Send(It.IsAny<CreateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateTodoItemResponse(domainTodoItem));

            var result = await _todoItemController
                .PostTodoItem(new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Description",
                    IsCompleted = false
                });

            result.Result
                .Should()
                .NotBeNull();
            
            result.Result
                .Should()
                .BeOfType<CreatedAtActionResult>();

            var createdResult = result.Result as CreatedAtActionResult;

            createdResult!.Value
                .Should()
                .BeEquivalentTo(_mapper.Map<TodoItem>(domainTodoItem));
        }

        [Fact]
        public async Task Given_PostTodoItem_When_SendCreateTodoItemCommandReturnsDuplicateError_Then_ReturnsBadRequestObjectResultWithErrorResponse()
        {
            _senderMock
                .Setup(x => x.Send(It.IsAny<CreateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DuplicateError(new Dictionary<string, string[]>()));

            var result = await _todoItemController
                .PostTodoItem(new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Description",
                    IsCompleted = false
                });

            result.Result
                .Should()
                .NotBeNull();
            
            result.Result
                .Should()
                .BeOfType<BadRequestObjectResult>();

            var createdResult = result
                .Result as BadRequestObjectResult;

            createdResult!.Value
                .Should()
                .BeEquivalentTo(new BadRequest());
        }

        [Fact]
        public async Task Given_PostTodoItem_When_SendCreateTodoItemCommandReturnsValidationError_Then_ReturnsBadRequestObjectResultWithErrorResponse()
        {
            _senderMock
                .Setup(x => x.Send(It.IsAny<CreateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationError(new Dictionary<string, string[]>()));

            var result = await _todoItemController
                .PostTodoItem(new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Description",
                    IsCompleted = false
                });

            result.Result
                .Should()
                .NotBeNull();
            
            result.Result
                .Should()
                .BeOfType<BadRequestObjectResult>();

            var createdResult = result
                .Result as BadRequestObjectResult;

            createdResult!.Value
                .Should()
                .BeEquivalentTo(new BadRequest());
        }

        [Fact]
        public async Task Given_PutTodoItem_When_SendUpdateTodoItemCommandUpdatesItem_Then_ReturnsNoContentResult()
        {
            var routeId = Guid.Parse("4a28d173-0c27-4d99-80e8-1aedc9d224a8");
            var itemId = Guid.Parse("4a28d173-0c27-4d99-80e8-1aedc9d224a8");
            var todoItem = new TodoItem
            {
                Id = itemId
            };

            _senderMock
                .Setup(x => x.Send(It.IsAny<UpdateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UpdateTodoItemResponse());

            var result = await _todoItemController
                .PutTodoItem(routeId, todoItem);

            result
                .Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Given_PutTodoItem_When_SendUpdateTodoItemCommandReturnsNotFoundError_Then_ReturnsNotFoundObjectResultWithErrorResponse()
        {
            var routeId = Guid.Parse("4a28d173-0c27-4d99-80e8-1aedc9d224a8");
            var itemId = Guid.Parse("4a28d173-0c27-4d99-80e8-1aedc9d224a8");
            var todoItem = new TodoItem
            {
                Id = itemId
            };

            _senderMock
                .Setup(x => x.Send(It.IsAny<UpdateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotFoundError(new Dictionary<string, string[]>()));

            var result = await _todoItemController
                .PutTodoItem(routeId, todoItem);

            result
                .Should()
                .NotBeNull();
            
            result
                .Should()
                .BeOfType<NotFoundObjectResult>();
            
            var badRequestObjectResult = result
                as NotFoundObjectResult;

            badRequestObjectResult!.Value
                .Should()
                .BeEquivalentTo(new NotFound());
        }

        [Fact]
        public async Task Given_PutTodoItem_When_SendUpdateTodoItemCommandReturnsValidationError_Then_ReturnsBadRequestObjectResultWithErrorResponse()
        {
            var routeId = Guid.Parse("4a28d173-0c27-4d99-80e8-1aedc9d224a8");
            var itemId = Guid.Parse("4a28d173-0c27-4d99-80e8-1aedc9d224a8");
            var todoItem = new TodoItem
            {
                Id = itemId
            };

            _senderMock
                .Setup(x => x.Send(It.IsAny<UpdateTodoItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationError(new Dictionary<string, string[]>()));

            var result = await _todoItemController
                .PutTodoItem(routeId, todoItem);

            result
                .Should()
                .NotBeNull();
            
            result
                .Should()
                .BeOfType<BadRequestObjectResult>();
            
            var badRequestObjectResult = result
                as BadRequestObjectResult;

            badRequestObjectResult!.Value
                .Should()
                .BeEquivalentTo(new BadRequest());
        }

        [Fact]
        public async Task Given_PutTodoItem_When_IdsMismatch_Then_ReturnsBadRequestObjectResultWithErrorResponse()
        {
            var routeId = Guid.Parse("4a28d173-0c27-4d99-80e8-1aedc9d224a8");
            var itemId = Guid.Parse("3a28d173-0c27-4d99-80e8-1aedc9d224a8");
            var todoItem = new TodoItem
            {
                Id = itemId
            };

            var result = await _todoItemController
                .PutTodoItem(routeId, todoItem);

            result
                .Should()
                .NotBeNull();
            
            result
                .Should()
                .BeOfType<BadRequestObjectResult>();
            
            var badRequestObjectResult = result
                as BadRequestObjectResult;

            badRequestObjectResult!.Value
                .Should()
                .BeEquivalentTo(new BadRequest());
        }
    }
}
