using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoList.Api.Common.Features
{
    [ExcludeFromCodeCoverage(Justification = "Context")]
    public class ExceptionContextFeature : IExceptionContextFeature
    {
        public ExceptionContext? ExceptionContext { get; set; }
    }
}
