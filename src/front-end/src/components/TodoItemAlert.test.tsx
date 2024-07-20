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

describe('Give a {status} and {error}', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    test('displays info alert for TodoItemStatus.None', () => {
        mockUseTodoContext.mockReturnValue({ 
            status: TodoItemStatus.None, 
            error: null 
        });
        
        render(<TodoItemAlert />);
        
        expect(screen.getByText(TodoItemStatusMessages.None))
            .toBeInTheDocument();
        expect(screen.getByRole('alert'))
            .toHaveClass('alert-info');
    });

    test('displays success alert for TodoItemStatus.Added', () => {
        mockUseTodoContext.mockReturnValue({ status: 
            TodoItemStatus.Added, 
            error: null 
        });
        
        render(<TodoItemAlert />);
        
        expect(screen.getByText(TodoItemStatusMessages.Added))
            .toBeInTheDocument();
        expect(screen.getByRole('alert'))
            .toHaveClass('alert-success');
    });

    test('displays success alert for TodoItemStatus.Completed', () => {
        mockUseTodoContext.mockReturnValue({ 
            status: TodoItemStatus.Completed, 
            error: null 
        });
        
        render(<TodoItemAlert />);
        
        expect(screen.getByText(TodoItemStatusMessages.Completed))
            .toBeInTheDocument();
        expect(screen.getByRole('alert'))
            .toHaveClass('alert-success');
    });

    test('displays danger alert for TodoItemStatus.Error', () => {
        const error: TodoItemError = {
            Title: 'Error Title',
            Errors: {
                field1: ['Error message 1', 'Error message 2'],
            },
            Type: 'http://example.com/error',
            Status: 500,
            TraceId: 'abc123',
        };

        mockUseTodoContext.mockReturnValue({ status: 
            TodoItemStatus.Error, 
            error 
        });
        
        render(<TodoItemAlert />);
        
        expect(screen.getByText(TodoItemStatusMessages.Error))
            .toBeInTheDocument();
        expect(screen.getByRole('alert'))
            .toHaveClass('alert-danger');

        fireEvent.click(screen.getByText("View Details"));
        
        expect(screen.getByText('Problem Details'))
            .toBeInTheDocument();
        expect(screen.getByText('Error Title'))
            .toBeInTheDocument();
        expect(screen.getByText('field1:'))
            .toBeInTheDocument();
        expect(screen.getByText('Error message 1'))
            .toBeInTheDocument();
        expect(screen.getByText('Error message 2'))
            .toBeInTheDocument();
    });

    test('displays info alert for TodoItemStatus.Refreshed', () => {
        mockUseTodoContext.mockReturnValue({ 
            status: TodoItemStatus.Refreshed, 
            error: null 
        });
        
        render(<TodoItemAlert />);
        
        expect(screen.getByText(TodoItemStatusMessages.Refreshed))
            .toBeInTheDocument();
        expect(screen.getByRole('alert'))
            .toHaveClass('alert-info');
    });
});
