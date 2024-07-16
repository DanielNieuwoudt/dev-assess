using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoList.Api.Common.Features;

public interface IExceptionContextFeature
{
    ExceptionContext? ExceptionContext { get; set; }
}