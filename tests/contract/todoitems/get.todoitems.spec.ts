import { createApiClient } from '../utils/Agent'
import { TodoItemsApi, TodoItemsApiInterface, TodoItem } from '../generated/';
import { AxiosResponse } from 'axios';
import { config } from '../config/base';
import { loadApiSpec } from '../utils/Specs';

describe('Given the backend endpoint',  () => {
    let todoApiInterface: TodoItemsApiInterface;
    let openApiSpecFile: string = 'todoitems.openapi.yaml'
    
    beforeEach(async () => {
        todoApiInterface = new TodoItemsApi(undefined, config.backendBaseUrl, createApiClient());
    });

    describe('When getting todo items',  () => {
        
        test('Then the client should receive a status code of 200 when the items are returned.', async() => {
            let todoItemsResponse: AxiosResponse<TodoItem[]>;
            
            todoItemsResponse = await todoApiInterface.getTodoItems();

            expect(todoItemsResponse.status).toBe(200);

            let todoItemsResults: TodoItem[] = todoItemsResponse.data;

            loadApiSpec(openApiSpecFile);
            expect(todoItemsResults).toSatisfySchemaInApiSpec('TodoItems');
        });
    });
});