import React, {FC, ReactNode, useEffect, useState} from 'react';
import { TodoItem } from '../services/generated';
import TodoApi from '../services/TodoApi';
import TodoContext from '../contexts/TodoContext';
import TodoItemStatus from '../enumerations/TodoItemStatus';
import { mapErrorResponseToTodoItemError } from '../services/TodoApiError';

export const TodoProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [items, setItems] = useState<TodoItem[]>([]);
    const [error, setError] = useState<TodoItemError | null>(null);
    const [status, setStatus] = useState<TodoItemStatus>(TodoItemStatus.None);
   
    const fetchItems = async () => {
        try {
            const fetchedItems = await TodoApi.getTodoItems();
            setItems(fetchedItems);
            setError(null);
        } catch (error: any) {
            setStatus(TodoItemStatus.Error);
            if (error.response == null ) {
                setError(null);    
            } else {
                setError(mapErrorResponseToTodoItemError(error.response.data));    
            }
        }
    };

    const addItem = async (todoItem: TodoItem) => {
        try {
            await TodoApi.postTodoItem(todoItem);
            await fetchItems();
            setError(null);
            setStatus(TodoItemStatus.Added);
        } catch (error: any) {
            setStatus(TodoItemStatus.Error);
            if (error.response == null) {
                setError(null);
            } else {
                setError(mapErrorResponseToTodoItemError(error.response.data));
            }
        }
    };

    const markItemAsComplete = async (id: string) => {
        try {
            const item = items.find(item => item.id === id);
            if (item) {
                item.isCompleted = true;
                await TodoApi.putTodoItem(id, item);
                await fetchItems();
                setError(null);
                setStatus(TodoItemStatus.Completed);
            }
        } catch (error: any) {
            setStatus(TodoItemStatus.Error);
            if (error.response == null) {
                setError(null);
            } else {
                setError(mapErrorResponseToTodoItemError(error.response.data));
            }
        }
    };

    const clearError = async() => {
        setError(null);
        setStatus(TodoItemStatus.None);
    }
    
    const setItemStatus = async (status: TodoItemStatus)=> {
        setStatus(status);
    }

    useEffect(() => {
        fetchItems();
    }, []);
    
    return (
        <TodoContext.Provider value={{items, error, status, addItem, clearError, fetchItems, markItemAsComplete, setItemStatus }}>
            {children}
        </TodoContext.Provider>
    );
};

export default TodoProvider;