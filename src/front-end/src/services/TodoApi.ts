import { TodoItemsApi, TodoItemsApiInterface, TodoItem } from './generated';
import { AxiosResponse } from 'axios';
import { createApiClient } from '../utils/Agent';
  
class TodoApi {
    static async getTodoItems(): Promise<TodoItem[]> {

        let todoApiInterface: TodoItemsApiInterface = new TodoItemsApi(undefined, this.getBackendUrl(), createApiClient());
        let response: AxiosResponse<TodoItem[]>;

        response = await todoApiInterface
            .getTodoItems();

        return response.data;
    }

    static async postTodoItem(todoItem: TodoItem): Promise<TodoItem> {

        let todoApiInterface: TodoItemsApiInterface = new TodoItemsApi(undefined, this.getBackendUrl(), createApiClient());
        let response: AxiosResponse<TodoItem>;

        response = await todoApiInterface
            .postTodoItem(todoItem);

        return response.data;
    }

    static async putTodoItem(id: string, todoItem: TodoItem): Promise<void> {

        let todoApiInterface: TodoItemsApiInterface = new TodoItemsApi(undefined, this.getBackendUrl(), createApiClient());
        let response: AxiosResponse<void>;

        response = await todoApiInterface
            .putTodoItem(id, todoItem);

        return response.data;
    }
    
    static getBackendUrl(): string {
        return window.env ? window.env.REACT_APP_BACKEND_BASE_URL : 'https://localhost:5001/api'
    }
}

export default TodoApi