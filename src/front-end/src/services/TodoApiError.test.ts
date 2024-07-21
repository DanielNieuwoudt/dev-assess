import { mapErrorResponseToTodoItemError } from './TodoApiError';

describe('Given error response When mapErrorResponseToTodoItemError Then', () => {
    it('should map the error response to TodoItemError correctly', () => {
        const responseData = {
            title: 'Error Title',
            type: 'ErrorType',
            status: 400,
            errors: ['Error1', 'Error2'],
            traceId: 'trace-id-1234',
        };

        const expectedTodoItemError = {
            Title: 'Error Title',
            Type: 'ErrorType',
            Status: 400,
            Errors: ['Error1', 'Error2'],
            TraceId: 'trace-id-1234',
        };

        const result = mapErrorResponseToTodoItemError(responseData);

        expect(result).toEqual(expectedTodoItemError);
    });

    it('should handle missing fields in the error response', () => {
        const responseData = {
            title: 'Error Title',
            status: 400,
            errors: ['Error1', 'Error2'],
        };

        const expectedTodoItemError = {
            Title: 'Error Title',
            Type: undefined,
            Status: 400,
            Errors: ['Error1', 'Error2'],
            TraceId: undefined,
        };

        const result = mapErrorResponseToTodoItemError(responseData);

        expect(result).toEqual(expectedTodoItemError);
    });
});