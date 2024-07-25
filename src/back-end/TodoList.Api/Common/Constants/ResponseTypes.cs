using System.Diagnostics.CodeAnalysis;

namespace TodoList.Api.Common.Constants
{
    [ExcludeFromCodeCoverage(Justification = "Non Functional Constants")]
    public class ResponseTypes
    {
        public const string BadRequest = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        public const string InternalServerError = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        public const string NotFound = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
    }
}
