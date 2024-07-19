import { TodoItemsApi, TodoItemsApiInterface, TodoItem } from './generated';
import { AxiosResponse } from 'axios';
import { IConfig} from '../config/base';
import { createApiClient } from '../utils/Agent';
  
class TodoApi {
  static async fetchItems(config: IConfig): Promise<TodoItem[]> {
    try {
      
      let todoApiInterface: TodoItemsApiInterface = new TodoItemsApi(undefined, config.backendBaseUrl, createApiClient());
      let todoItemsResponse: AxiosResponse<TodoItem[]>;

      console.log(`Retrieving todo items from ${config.backendBaseUrl}`)
      
      todoItemsResponse = await todoApiInterface.getTodoItems();

      if (Array.isArray(todoItemsResponse.data)) {
        return todoItemsResponse.data;
      } else {
        console.error('API response is not an array:', todoItemsResponse.data);
        return [];
      }
    } catch (error) {
      console.error(error);
      return [];
    }
  }
}

export default TodoApi