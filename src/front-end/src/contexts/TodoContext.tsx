import React, { createContext, useContext, useState, FC, ReactNode } from 'react';
import { TodoItem } from '../services/generated';
import TodoApi from '../services/TodoApi';

interface TodoContextProps {
    items: TodoItem[];
    addItem: (todoItem: TodoItem) => Promise<void>;
    markItemAsComplete: (id: string) => Promise<void>;
    fetchItems: () => Promise<void>;
}

const TodoContext = createContext<TodoContextProps | undefined>(undefined);

export const useTodoContext = () => {
    const context = useContext(TodoContext);
    if (!context) {
        throw new Error('useTodoContext must be used within a TodoProvider');
    }
    return context;
};

export const TodoProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [items, setItems] = useState<TodoItem[]>([]);

    const fetchItems = async () => {
        try {
            const fetchedItems = await TodoApi.getTodoItems();
            setItems(fetchedItems);
        } catch (error) {
            console.error(error);
        }
    };

    const addItem = async (todoItem: TodoItem) => {
        try {
            await TodoApi.postTodoItem(todoItem);
            await fetchItems();
        } catch (error) {
            console.error(error);
        }
    };

    const markItemAsComplete = async (id: string) => {
        try {
            const item = items.find(item => item.id === id);
            if (item) {
                item.isCompleted = true;
                await TodoApi.putTodoItem(id, item);
                await fetchItems();
            }
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <TodoContext.Provider value={{ items, addItem, markItemAsComplete, fetchItems }}>
            {children}
        </TodoContext.Provider>
    );
};
