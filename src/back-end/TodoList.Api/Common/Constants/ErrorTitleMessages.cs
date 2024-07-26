using System.Diagnostics.CodeAnalysis;

namespace TodoList.Api.Common.Constants
{
    [ExcludeFromCodeCoverage(Justification = "Constant by definition")]
    public class ErrorTitleMessages
    {
        public const string NotFound = "The specified resource was not found.";
        public const string ValidationError = "One or more validation errors has occured.";
        public const string InternalServerError = "An error occurred.";
    }
}
