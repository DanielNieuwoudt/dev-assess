using System.Diagnostics.CodeAnalysis;

namespace TodoList.Application.Common.Enumerations;

public enum ErrorReason
{
    Duplicate,
    NotFound,
    Validation
}