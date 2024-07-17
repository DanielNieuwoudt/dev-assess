import { createApiClient } from '../utils/Agent'
import {createNewTodo} from "../utils/Todo";
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

    describe("When getting a todo item",  () => {

        // We generate a uuid for the todoItem id.
        let todoItemId: string = uuidv4();
        // We generate a uuid as a random description.
        let todoItemDescription: string = uuidv4();
        
        test("Then the client should receive a status code of 404 when an {id} does not exist.", async () => {
            let todoItemResponse: AxiosResponse<TodoItem>;
            let todoItemId: string = uuidv4();
           
            todoItemResponse = await todoApiInterface.getTodoItem(todoItemId);
            expect(todoItemResponse.status).toBe(404);

            let todoItemResult: TodoItem = todoItemResponse.data;
            
            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemResult).toSatisfySchemaInApiSpec("NotFound");
        });

        test("Then the client should receive a status code of 201 when the todo item is created.", async() => {
            let todoItemCreateResponse: AxiosResponse<TodoItem>;

            todoItemCreateResponse = await todoApiInterface
                .postTodoItem(createNewTodo(todoItemId, todoItemDescription));

            expect(todoItemCreateResponse.status).toBe(201);

            let todoItemCreateResult: TodoItem = todoItemCreateResponse.data;

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemCreateResult).toSatisfySchemaInApiSpec("TodoItem");
        });

        test("Then the client should receive a status code of 200 when the {id} is valid.", async () => {
            let todoItemResponse: AxiosResponse<TodoItem>;

            todoItemResponse = await todoApiInterface.getTodoItem(todoItemId);
            expect(todoItemResponse.status).toBe(200);

            let todoItemResult: TodoItem = todoItemResponse.data;

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemResult).toSatisfySchemaInApiSpec("TodoItem");
        });
    });
});