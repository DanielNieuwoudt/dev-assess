using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors
{
    public abstract class ApplicationError
    {
        protected ApplicationError(ErrorReason reason, string property, string message)
        {
            Errors = new Dictionary<string, string[]>
            {
                { property, new[] { message } }
            };
            Reason = reason;

        }
        
        protected ApplicationError(ErrorReason reason, IDictionary<string, string[]> errors)
        {
            Errors = errors;
            Reason = reason;
        }

        public IDictionary<string, string[]> Errors { get; }

        public ErrorReason Reason { get; }
    };
}
