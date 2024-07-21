import { mapErrorResponseToTodoItemError } from '../services/TodoApiError';

describe('Given response data When mapErrorResponseToTodoItemError Then', () => {
    it('should map the error response to TodoItemError correctly', () => {
        const responseData = {
            title: 'Error Title',
            type: 'ErrorType',
            status: 400,
            errors: {
                field1: ['Error1', 'Error2'],
                field2: ['Error3']
            },
            traceId: 'trace-id-1234',
        };

        const expectedTodoItemError: TodoItemError = {
            Title: 'Error Title',
            Type: 'ErrorType',
            Status: 400,
            Errors: {
                field1: ['Error1', 'Error2'],
                field2: ['Error3']
            },
            TraceId: 'trace-id-1234',
        };

        const result = mapErrorResponseToTodoItemError(responseData);

        expect(result).toEqual(expectedTodoItemError);
    });

    it('should handle missing fields in the error response', () => {
        const responseData = {
            title: 'Error Title',
            type: 'ErrorType',
            status: 400,
            traceId: 'trace-id-1234',
        };

        const expectedTodoItemError: TodoItemError = {
            Title: 'Error Title',
            Type: 'ErrorType',
            Status: 400,
            Errors: undefined!,
            TraceId: 'trace-id-1234',
        };

        const result = mapErrorResponseToTodoItemError(responseData);

        expect(result).toEqual(expectedTodoItemError);
    });
});
