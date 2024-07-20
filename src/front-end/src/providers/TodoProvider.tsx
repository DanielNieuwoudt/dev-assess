import React, {FC, ReactNode, useState} from "react";
import {TodoItem} from "../services/generated";
import TodoApi from "../services/TodoApi";
import TodoContext from "../contexts/TodoContext"
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
        <TodoContext.Provider value={{items, addItem, markItemAsComplete, fetchItems}}>
            {children}
        </TodoContext.Provider>
    );
};

export default TodoProvider;