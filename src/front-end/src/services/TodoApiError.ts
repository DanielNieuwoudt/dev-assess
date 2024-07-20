export function mapErrorResponseToTodoItemError(responseData: any): TodoItemError {
    return {
        Title: responseData.title,
        Type: responseData.type,
        Status: responseData.status,
        Errors: responseData.errors,
        TraceId: responseData.traceId,
    };
}