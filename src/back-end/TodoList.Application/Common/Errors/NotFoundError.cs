using System.Diagnostics.CodeAnalysis;
using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors;

[ExcludeFromCodeCoverage(Justification = "Record")]
public sealed class NotFoundError(string Property, string Message) : ApplicationError(ErrorReason.NotFound, Property, Message);