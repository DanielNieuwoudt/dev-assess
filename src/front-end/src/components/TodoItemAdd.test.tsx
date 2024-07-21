import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import TodoItemAdd from './TodoItemAdd';
import { useTodoContext } from '../contexts/TodoContext';
import TodoItemStatusMessages from "../constants/TodoItemStatusMessages";
import TodoItemStatus from "../enumerations/TodoItemStatus";

jest.mock('../contexts/TodoContext', () => ({
    useTodoContext: jest.fn(),
}));

const mockUseTodoContext = useTodoContext as jest.Mock;

describe('Given a new todo item When TodoItemAdd rendered Then', () => {
    const mockAddItem = jest.fn();
    const mockClearError = jest.fn();
    
    beforeEach(() => {
        mockUseTodoContext.mockReturnValue({
            clearError: mockClearError,
            addItem: mockAddItem
        });
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renders the component and initial elements', () => {
        render(<TodoItemAdd />);

        expect(screen.getByText('Add Item'))
            .toBeInTheDocument();
        expect(screen.getByPlaceholderText('Enter description...'))
            .toBeInTheDocument();
        expect(screen.getByText('Clear'))
            .toBeInTheDocument();
    });

    it('adds an item when the {Add Item} button is clicked', async () => {
        render(<TodoItemAdd />);

        const descriptionInput = screen
            .getByPlaceholderText('Enter description...');
        const addButton = screen
            .getByText('Add Item');

        fireEvent.change(descriptionInput, { target: { value: 'New Todo Item' } });
        fireEvent.click(addButton);

        await waitFor(() => {
            expect(mockAddItem).toHaveBeenCalledWith(expect.objectContaining({
                description: 'New Todo Item',
                isCompleted: false,
            }));
            expect(descriptionInput)
                .toHaveValue('');
        });
    });

    it('clears the {description} and {error} when the {Clear} button is clicked', async () => {
        render(<TodoItemAdd />);

        const descriptionInput = screen
            .getByPlaceholderText('Enter description...');
        const clearButton = screen
            .getByText('Clear');

        fireEvent.change(descriptionInput, { target: { value: 'New Todo Item' } });
        fireEvent.click(clearButton);

        await waitFor(() => {
            expect(descriptionInput)
                .toHaveValue('');
            expect(mockClearError)
                .toHaveBeenCalled();
        });
    });

    const testCases = [
        { label: 'None', status: TodoItemStatus.None, message: TodoItemStatusMessages.None },
        { label: 'Added', status: TodoItemStatus.Added, message: TodoItemStatusMessages.Added },
        { label: 'Completed', status: TodoItemStatus.Completed, message: TodoItemStatusMessages.Completed },
        { label: 'Error', status: TodoItemStatus.Error, message: TodoItemStatusMessages.Error },
        { label: 'Refreshed', status: TodoItemStatus.Refreshed, message: TodoItemStatusMessages.Refreshed },
    ];

    test.each(testCases)(
        'renders the TodoItemAlert component with status $label',
        ({ status, message }) => {
            mockUseTodoContext.mockReturnValue({
                clearError: mockClearError,
                addItem: mockAddItem,
                status,
                error: null,
            });

            render(<TodoItemAdd />);

            expect(screen.getByRole('alert'))
                .toBeInTheDocument();
            expect(screen.getByText(message))
                .toBeInTheDocument();
        }
    );
});
