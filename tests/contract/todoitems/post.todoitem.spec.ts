import { createApiClient } from '../utils/Agent'
import { createNewTodo } from '../utils/Todo';
import { TodoItemsApi, TodoItemsApiInterface, TodoItem } from '../generated/';
import { AxiosResponse } from 'axios';
import { config } from '../config/base';
import { loadApiSpec } from '../utils/Specs';
import { v4 as uuidv4 } from 'uuid';

describe('Given the backend endpoint',  () => {
    let todoApiInterface: TodoItemsApiInterface;
    let openApiSpecFile: string = 'todoitems.openapi.yaml'
    
    beforeEach(async () => {
        todoApiInterface = new TodoItemsApi(undefined, config.backendBaseUrl, createApiClient());
    });

    describe('When creating a todo item',  () => {
        
        // We generate a uuid for the todoItem id.
        let todoItemId: string = uuidv4();
        // We generate a uuid as a random description.
        let todoItemDescription: string = uuidv4();
        
        test('Then the client should receive a status code of 201 when the todo item is created.', async() => {
            let todoItemCreateResponse: AxiosResponse<TodoItem>;
            
            todoItemCreateResponse = await todoApiInterface
                .postTodoItem(createNewTodo(todoItemId, todoItemDescription));

            expect(todoItemCreateResponse.status).toBe(201);

            let todoItemCreateResult: TodoItem = todoItemCreateResponse.data;

            loadApiSpec(openApiSpecFile);
            expect(todoItemCreateResult).toSatisfySchemaInApiSpec('TodoItem');
        });

        test('Then the client should receive a status code of 400 when the {id} is a duplicate', async() => {

            let todoItemCreateResponse: AxiosResponse<TodoItem>;

            todoItemCreateResponse = await todoApiInterface
                .postTodoItem(createNewTodo(todoItemId, uuidv4()));

            expect(todoItemCreateResponse.status).toBe(400);

            let todoItemCreateResult: TodoItem = todoItemCreateResponse.data;

            loadApiSpec(openApiSpecFile);
            expect(todoItemCreateResult).toSatisfySchemaInApiSpec('BadRequest');
        });

        test('Then the client should receive a status code of 400 when the {description} is a duplicate', async() => {

            let todoItemCreateResponse: AxiosResponse<TodoItem>;

            todoItemCreateResponse = await todoApiInterface
                .postTodoItem(createNewTodo(uuidv4(), todoItemDescription));

            expect(todoItemCreateResponse.status).toBe(400);

            let todoItemCreateResult: TodoItem = todoItemCreateResponse.data;

            loadApiSpec(openApiSpecFile);
            expect(todoItemCreateResult).toSatisfySchemaInApiSpec('BadRequest');
        });
    });
});
