import React from 'react';
import { render, waitFor, screen, fireEvent } from '@testing-library/react';
import TodoProvider from './TodoProvider';
import TodoContext, {useTodoContext} from '../contexts/TodoContext';
import TodoApi from '../services/TodoApi';
import { TodoItem } from '../services/generated';
import TodoItemStatus from '../enumerations/TodoItemStatus';

// Mock the TodoApi
jest.mock('../services/TodoApi');

const mockedTodoApi = TodoApi as jest.Mocked<typeof TodoApi>;

const renderWithProvider = (ui: React.ReactElement) => {
    return render(
        <TodoProvider>
            {ui}
        </TodoProvider>
    );
};

describe('Given TodoProvider When component rendered Then', () => {
    beforeEach(() => {
        mockedTodoApi.getTodoItems.mockClear();
        mockedTodoApi.postTodoItem.mockClear();
        mockedTodoApi.putTodoItem.mockClear();
    });

    test('fetches and displays todo items on load', async () => {
        const items: TodoItem[] = [
            { id: '1', description: 'Test Item 1', isCompleted: false },
            { id: '2', description: 'Test Item 2', isCompleted: false },
        ];

        mockedTodoApi.getTodoItems
            .mockResolvedValue(items);

        renderWithProvider(
            <TodoContext.Consumer>
                {({ items } = useTodoContext()) => (
                    <ul>
                        {items.map(item => (
                            <li key={item.id}>{item.description}</li>
                        ))}
                    </ul>
                )}
            </TodoContext.Consumer>
        );

        await waitFor(() => {
            expect(screen.getByText('Test Item 1'))
                .toBeInTheDocument();
            expect(screen.getByText('Test Item 2'))
                .toBeInTheDocument();
        });
    });

    test('adds a new todo item', async () => {
        const newItem: TodoItem = { id: '3', description: 'New Item', isCompleted: false };
        
        mockedTodoApi.postTodoItem
            .mockResolvedValueOnce(newItem);
        mockedTodoApi.getTodoItems
            .mockResolvedValueOnce([newItem]);

        renderWithProvider(
            <TodoContext.Consumer>
                {({ addItem }  = useTodoContext()) => (
                    <button onClick={() => addItem(newItem)}>Add Item</button>
                )}
            </TodoContext.Consumer>
        );

        fireEvent.click(screen.getByText('Add Item'));

        await waitFor(() => {
            expect(mockedTodoApi.postTodoItem)
                .toHaveBeenCalledWith(newItem);
            expect(mockedTodoApi.getTodoItems)
                .toHaveBeenCalledTimes(2);
        });
    });

    test('marks an item as complete', async () => {
        const items: TodoItem[] = [
            { id: '1', description: 'Incomplete Item', isCompleted: false },
        ];
        mockedTodoApi.getTodoItems
            .mockResolvedValue(items);
        mockedTodoApi.putTodoItem
            .mockResolvedValueOnce();

        renderWithProvider(
            <TodoContext.Consumer>
                {({ items, fetchItems, markItemAsComplete }  = useTodoContext()) => (
                    <div>
                        <button onClick={() => fetchItems()}>Fetch Items</button>
                        <ul>
                            {items.map(item => (
                                <li key={item.id}>{item.description}</li>
                            ))}
                        </ul>
                        <button onClick={() => markItemAsComplete('1')}>Complete Item</button>
                    </div>

                )}
            </TodoContext.Consumer>
        );

        fireEvent.click(screen.getByText('Fetch Items'));

        await waitFor(() => {
            expect(screen.getByText('Incomplete Item'))
                .toBeInTheDocument();
        });

        fireEvent.click(screen.getByText('Complete Item'));

        await waitFor(() => {
            expect(mockedTodoApi.putTodoItem).toHaveBeenCalledWith('1', { ...items[0], isCompleted: true });
            expect(mockedTodoApi.getTodoItems).toHaveBeenCalledTimes(2);
        });
    });

    test('handles error when fetching items fails', async () => {
        mockedTodoApi.getTodoItems.mockRejectedValue(new Error('Failed to fetch items'));

        renderWithProvider(
            <TodoContext.Consumer>
                {({ status }  = useTodoContext()) => (
                    <div>{status === TodoItemStatus.Error && 'Error fetching items'}</div>
                )}
            </TodoContext.Consumer>
        );

        await waitFor(() => {
            expect(screen.getByText('Error fetching items')).toBeInTheDocument();
        });
    });
});
