import { createContext, useContext } from 'react';
import { TodoItem } from '../services/generated';
import TodoItemStatus from '../enumerations/TodoItemStatus';

interface TodoContextProps {
    items: TodoItem[];
    error: TodoItemError | null;
    status: TodoItemStatus;
    addItem: (todoItem: TodoItem) => Promise<void>;
    clearError: () => Promise<void>;
    fetchItems: () => Promise<void>;
    markItemAsComplete: (id: string) => Promise<void>;
    setItemStatus: (status: TodoItemStatus) => Promise<void>;
}

const TodoContext = createContext<TodoContextProps | undefined>(undefined);

export const useTodoContext = () => {
    const context = useContext(TodoContext);
    if (!context) {
        throw new Error('useTodoContext must be used within a TodoProvider');
    }
    return context;
};

export default TodoContext;
