using System.Diagnostics.CodeAnalysis;

namespace TodoList.Api.Common.Constants
{
    [ExcludeFromCodeCoverage(Justification = "Constant by definition")]
    public class ErrorDetailMessages
    {
        public const string IdMismatch = "The 'id' in the path does not match the item 'id'";
        public const string IdDoesNotExist = "The 'id' provided does not exist.";
        public const string PropertyDuplicate = "The provided property is a duplicate.";
        public const string SeeErrors = "See the errors property for details.";
        public const string ErrorProcessingRequest = "An error occurred processing your request.";
    }
}
