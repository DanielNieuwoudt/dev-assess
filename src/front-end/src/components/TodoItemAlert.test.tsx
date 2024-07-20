import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import TodoItemAlert from './TodoItemAlert';
import TodoItemStatus from '../enumerations/TodoItemStatus';
import { useTodoContext } from '../contexts/TodoContext';
import TodoItemStatusMessages from "../constants/TodoItemStatusMessages";

jest.mock('../contexts/TodoContext', () => ({
    useTodoContext: jest.fn(),
}));

const mockUseTodoContext = useTodoContext as jest.Mock;

describe('Given a {status} and {error} When rendering the TodoItemAlert component Then', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    const testCases = [
        { label: 'None', status: TodoItemStatus.None, error: null, message: TodoItemStatusMessages.None, alertClass: 'alert-info' },
        { label: 'Added', status: TodoItemStatus.Added, error: null, message: TodoItemStatusMessages.Added, alertClass: 'alert-success' },
        { label: 'Completed', status: TodoItemStatus.Completed, error: null, message: TodoItemStatusMessages.Completed, alertClass: 'alert-success' },
        { label: 'Error', status: TodoItemStatus.Error, error: {
                Title: 'Error Title',
                Errors: { field1: ['Error message 1', 'Error message 2'] },
                Type: 'http://example.com/error',
                Status: 500,
                TraceId: 'abc123'
            }, message: TodoItemStatusMessages.Error, alertClass: 'alert-danger' },
        { label: 'Refreshed', status: TodoItemStatus.Refreshed, error: null, message: TodoItemStatusMessages.Refreshed, alertClass: 'alert-info' }
    ];

    test.each(testCases)(
        'displays the correct alert for $label',
        ({ status, error, message, alertClass }) => {
            mockUseTodoContext.mockReturnValue({ status, error });

            render(<TodoItemAlert />);

            expect(screen.getByText(message)).toBeInTheDocument();
            expect(screen.getByRole('alert')).toHaveClass(alertClass);

            if (error) {
                fireEvent.click(screen.getByText("View Details"));

                expect(screen.getByText('Problem Details')).toBeInTheDocument();
                expect(screen.getByText(error.Title)).toBeInTheDocument();
                expect(screen.getByText('field1:')).toBeInTheDocument();
                expect(screen.getByText('Error message 1')).toBeInTheDocument();
                expect(screen.getByText('Error message 2')).toBeInTheDocument();
            }
        }
    );
});