import React from 'react';
import {render, screen, fireEvent, waitFor} from '@testing-library/react';
import TodoItems from './TodoItems';
import { useTodoContext } from '../contexts/TodoContext';
import TodoItemStatus from '../enumerations/TodoItemStatus';

jest.mock('../contexts/TodoContext', () => ({
    useTodoContext: jest.fn(),
}));

const mockUseTodoContext = useTodoContext as jest.Mock;

// Mock data for tests
const mockItems = [
    { id: '1', description: 'Test Item 1', isCompleted: false },
    { id: '2', description: 'Test Item 2', isCompleted: true },
    { id: '3', description: 'Test Item 3', isCompleted: false }
];

const renderWithContext = (contextValue: any) => {
    mockUseTodoContext.mockReturnValue(contextValue);
    render(<TodoItems />);
};

describe('Given todo items When rendering the TodoItems component Then', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    test('displays the correct number of incomplete items', () => {
        
        const context = {
            items: mockItems,
            fetchItems: jest.fn(),
            markItemAsComplete: jest.fn(),
            setItemStatus: jest.fn()
        };
        
        renderWithContext(context);

        expect(screen.getByText('Showing 2 Item(s)'))
            .toBeInTheDocument();
        expect(screen.getByText('Test Item 1'))
            .toBeInTheDocument();
        expect(screen.queryByText('Test Item 2'))
            .not.toBeInTheDocument(); // Completed item should not be displayed
        expect(screen.getByText('Test Item 3'))
            .toBeInTheDocument();
    });

    test('{refreshItems} function is called when {Refresh} button is clicked', async () => {
        
        const context = {
            items: mockItems,
            fetchItems: jest.fn(),
            markItemAsComplete: jest.fn(),
            setItemStatus: jest.fn()
        };
        
        renderWithContext(context);

        const refreshButton = screen.getByText('Refresh');
        
        fireEvent.click(refreshButton);

        await waitFor( () => {
            expect(context.fetchItems)
                .toHaveBeenCalled();
            expect(context.setItemStatus)
                .toHaveBeenCalledWith(TodoItemStatus.Refreshed);
        });
    });

    test('{markItemAsComplete} function is called when {Mark as completed} button is clicked', async () => {

        const context = {
            items: mockItems,
            fetchItems: jest.fn(),
            markItemAsComplete: jest.fn(),
            setItemStatus: jest.fn()
        };
        
        renderWithContext(context);

        const markAsCompleteButtons = screen.getAllByText('Mark as completed');
        
        fireEvent.click(markAsCompleteButtons[0]);

        await waitFor(() => {
            expect(context.markItemAsComplete)
                .toHaveBeenCalledWith('1');
        });
    });
});
