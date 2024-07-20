import React, { createContext, useContext, useState, FC, ReactNode } from 'react';
import { TodoItem } from '../services/generated';

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

export default TodoContext;
