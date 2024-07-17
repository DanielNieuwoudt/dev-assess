import { createApiClient } from '../utils/Agent'
import { createNewTodo } from "../utils/Todo";
import { TodoApi, TodoApiInterface, TodoItem } from "../generated/";
import { AxiosResponse } from "axios";
import { config } from "../config/base";
import { loadApiSpec } from "../utils/Specs";
import { v4 as uuidv4 } from 'uuid';

describe("Given the backend endpoint",  () => {
    let todoApiInterface: TodoApiInterface;
    beforeEach(async () => {
        todoApiInterface = new TodoApi(undefined, config.backendBaseUrl, createApiClient());
    });
  
    describe("When creating and then updating a todo item",  () => {
        
        // We generate a uuid for the todoItem id.
        let todoItemId: string = uuidv4();
        // We generate a uuid as a random description.
        let todoItemDescription: string = uuidv4();
        
        test("Then the client should receive a status code of 201 when the todo item is created.", async() => {
            let todoItemCreateResponse: AxiosResponse<TodoItem>;
            
            todoItemCreateResponse = await todoApiInterface
                .postTodoItem(createNewTodo(todoItemId, todoItemDescription));

            expect(todoItemCreateResponse.status).toBe(201);

            let todoItemCreateResult: TodoItem = todoItemCreateResponse.data;

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemCreateResult).toSatisfySchemaInApiSpec("TodoItem");
        });

        test("Then the client should receive a status code of 400 when the {routeId} and {todoItemId} do not match", async() => {

            let todoItemUpdateResponse: AxiosResponse<void>;

            todoItemUpdateResponse = await todoApiInterface
                .putTodoItem(uuidv4(), createNewTodo(todoItemId, uuidv4()));

            expect(todoItemUpdateResponse.status).toBe(400);

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemUpdateResponse.data).toSatisfySchemaInApiSpec("BadRequest");
        });

        test("Then the client should receive a status code of 400 when the {description} is empty.", async() => {

            let todoItemUpdateResponse: AxiosResponse<void>;

            todoItemUpdateResponse = await todoApiInterface
                .putTodoItem(uuidv4(), createNewTodo(todoItemId, ''));

            expect(todoItemUpdateResponse.status).toBe(400);

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemUpdateResponse.data).toSatisfySchemaInApiSpec("BadRequest");
        });

        test("Then the client should receive a status code of 404 when the todo item is not found.", async() => {

            let todoItemUpdateResponse: AxiosResponse<void>;
            let nonExistingTodoItemId: string = uuidv4();
            
            todoItemUpdateResponse = await todoApiInterface
                .putTodoItem(nonExistingTodoItemId, createNewTodo(nonExistingTodoItemId, todoItemDescription));

            expect(todoItemUpdateResponse.status).toBe(404);

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemUpdateResponse.data).toSatisfySchemaInApiSpec("NotFound");
        });
        
        test("Then the client should receive a status code of 204 when the body is valid.", async() => {

            let todoItemUpdateResponse: AxiosResponse<void>;

            todoItemUpdateResponse = await todoApiInterface
                .putTodoItem(todoItemId, createNewTodo(todoItemId, todoItemDescription));

            expect(todoItemUpdateResponse.status).toBe(204);
        });
    });
});

