using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Api.Common.Constants;
using TodoList.Api.Common.Helpers;
using TodoList.Application.Common.Errors;
using Xunit;

namespace TodoList.Api.Tests.Common.Helpers
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class ErrorHelperTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new ();
        private readonly NullLogger<ErrorHelper> _nullLogger = new ();
        private readonly ErrorHelper _errorHelper;
        private readonly IDictionary<string, string[]> _errors = new Dictionary<string, string[]>
        {
            { "property", new [] { "message"} }
        };

        public ErrorHelperTests()
        {
            _errorHelper = new ErrorHelper(_httpContextAccessorMock.Object, _nullLogger);
        }

        [Fact]
        public void Given_NullHttpContextAccessor_When_ErrorHelperInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new ErrorHelper(null!, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_ErrorHelperInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new ErrorHelper(_httpContextAccessorMock.Object, null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullDuplicateError_When_DuplicateErrorResult_Then_ThrowsArgumentNullException()
        {
            var action = () => _errorHelper.DuplicateErrorResult(null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullHttpContext_When_DuplicateErrorResult_Then_ThrowsArgumentNullException()
        {
            SetHttpContext(true);

            var action = () => _errorHelper.DuplicateErrorResult(new DuplicateError("property", "message"));

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_DuplicateError_When_DuplicateErrorResult_Then_ReturnsBadRequestObjectResultWithErrorResponse()
        {
            SetHttpContext(false);

            var expectedResponse = new Generated.BadRequest
            {
                Title = ErrorTitleMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorDetailMessages.PropertyDuplicate,
                Errors = _errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = "ABC"
            };

            var result = _errorHelper.DuplicateErrorResult(new DuplicateError("property", "message"));

            _httpContextAccessorMock.Object.HttpContext!
                .Response
                .ContentType
                .Should()
                .Be(MediaTypes.ApplicationProblemJson);

            result
                .Should()
                .BeOfType<BadRequestObjectResult>();
            
            result.Value
                .Should()
                .BeOfType<Generated.BadRequest>();

            var badRequest = result
                .Value as Generated.BadRequest;

            badRequest
                .Should()
                .BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public void Given_NullNotFoundError_When_NotFoundErrorResult_Then_ThrowsArgumentNullException()
        {
            var action = () => _errorHelper.NotFoundErrorResult(null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullHttpContext_When_NotFoundErrorResult_Then_ThrowsArgumentNullException()
        {
            SetHttpContext(true);

            var action = () => _errorHelper.NotFoundErrorResult(new NotFoundError("property", "message"));

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NotFoundError_When_NotFoundErrorResult_Then_ReturnsNotFoundObjectResultWithErrorResponse()
        {
            SetHttpContext(false);

            var expectedResponse = new Generated.NotFound
            {
                Title = ErrorTitleMessages.NotFound,
                Detail = ErrorDetailMessages.IdDoesNotExist,
                Type = ResponseTypes.NotFound,
                Status = StatusCodes.Status404NotFound,
                TraceId = "ABC"
            };

            var result = _errorHelper.NotFoundErrorResult(new NotFoundError("property", "message"));

            _httpContextAccessorMock.Object.HttpContext!
                .Response
                .ContentType
                .Should()
                .Be(MediaTypes.ApplicationProblemJson);

            result
                .Should()
                .BeOfType<NotFoundObjectResult>();
            
            result.Value
                .Should()
                .BeOfType<Generated.NotFound>();

            var notFound = result
                .Value as Generated.NotFound;

            notFound
                .Should()
                .BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public void Given_NullValidationError_When_ValidationErrorResult_Then_ThrowsArgumentNullException()
        {
            var action = () => _errorHelper.ValidationErrorResult(null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullHttpContext_When_ValidationErrorResult_Then_ThrowsArgumentNullException()
        {
            SetHttpContext(true);

            var action = () => _errorHelper.ValidationErrorResult(new ValidationError(_errors));

            action
                .Should()
                .Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void Given_ValidationError_When_ValidationErrorResult_Then_ReturnsBadRequestObjectResultWithErrorResponse()
        {
            SetHttpContext(false);

            var expectedResponse = new Generated.BadRequest
            {
                Title = ErrorTitleMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorDetailMessages.SeeErrors,
                Errors = _errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = "ABC"
            };

            var result = _errorHelper.ValidationErrorResult(new ValidationError(_errors));

            _httpContextAccessorMock.Object.HttpContext!
                .Response
                .ContentType
                .Should()
                .Be(MediaTypes.ApplicationProblemJson);

            result
                .Should()
                .BeOfType<BadRequestObjectResult>();
            
            result.Value
                .Should()
                .BeOfType<Generated.BadRequest>();

            var badRequest = result
                .Value as Generated.BadRequest;

            badRequest
                .Should()
                .BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public void Given_NullHttpContext_When_IdMismatchValidationError_Then_ThrowsArgumentNullException()
        {
            SetHttpContext(true);

            var action = () => _errorHelper.IdMismatchValidationError();

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_IdMismatch_When_IdMismatchValidationError_Then_ReturnsBadRequestObjectResultWithErrorResponse()
        {
            SetHttpContext(false);

            var expectedResponse = new Generated.BadRequest
            {
                Title = ErrorTitleMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorDetailMessages.IdMismatch,
                Errors = new Dictionary<string, List<string>>(),
                TraceId = "ABC"
            };

            var result = _errorHelper.IdMismatchValidationError();

            _httpContextAccessorMock.Object.HttpContext!
                .Response
                .ContentType
                .Should()
                .Be(MediaTypes.ApplicationProblemJson);

            result
                .Should()
                .BeOfType<BadRequestObjectResult>();
            
            result.Value
                .Should()
                .BeOfType<Generated.BadRequest>();

            var badRequest = result
                .Value as Generated.BadRequest;

            badRequest
                .Should()
                .BeEquivalentTo(expectedResponse);
        }

        private void SetHttpContext(bool isNull)
        {
            _httpContextAccessorMock.Reset();
            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(isNull
                    ? default
                    : new DefaultHttpContext
                    {
                        TraceIdentifier = "ABC"
                    });
        }
    }
}
