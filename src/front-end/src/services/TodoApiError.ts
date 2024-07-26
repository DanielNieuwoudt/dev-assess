export function mapErrorResponseToTodoItemError(responseData: any): TodoItemError {
    return {
        Title: responseData.title,
        Type: responseData.type,
        Detail: responseData.detail,
        Status: responseData.status,
        Errors: responseData.errors,
        TraceId: responseData.traceId,
    };
}