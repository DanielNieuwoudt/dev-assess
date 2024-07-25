namespace TodoList.Api.Common.Constants
{
    public class ErrorMessages
    {
        public const string NotFound = "The specified resource was not found.";
        public const string ValidationError = "One or more validation errors has occured.";
        public const string InternalServerError = "An error occurred.";

        public const string IdMismatch = "The 'id' in the path does not match the item 'id'";
        public const string IdDoesNotExist = "The 'id' provided does not exist.";
        public const string PropertyDuplicate = "The provided property is a duplicate.";
        public const string SeeErrors = "See the errors property for details.";
        public const string ErrorProcessingRequest = "An error occurred processing your request.";
    }
}
