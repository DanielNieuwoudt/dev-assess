import { createApiClient } from '../utils/Agent'
import { TodoApi, TodoApiInterface, TodoItem } from "../generated/";
import { AxiosResponse } from "axios";
import { config } from "../config/base";
import { loadApiSpec } from "../utils/Specs";
import { v4 as uuidv4 } from 'uuid'

describe("Given the backend endpoint",  () => {
    let todoApiInterface: TodoApiInterface;
    beforeEach(async () => {
        todoApiInterface = new TodoApi(undefined, config.backendBaseUrl, createApiClient());
    });

    describe("When getting todo items",  () => {
        
        test("Then the client should receive a status code of 200 and the response data should match the TodoItems schema.", async() => {
            let todoItemsResponse: AxiosResponse<TodoItem[]>;
            
            todoItemsResponse = await todoApiInterface.getTodoItems();

            expect(todoItemsResponse.status).toBe(200);

            let todoItemsResults: TodoItem[] = todoItemsResponse.data;

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemsResults).toSatisfySchemaInApiSpec("TodoItems");
        });
    });

    describe("When getting a todo item",  () => {

        test("Then the client should receive a status code of 404 when an {id} does not exist.", async () => {
            let todoItemResponse: AxiosResponse<TodoItem>;
            let todoItemId: string;

            todoItemId = "ee485473-aeb7-4960-a784-8bf97705e7b9"
            todoItemResponse = await todoApiInterface.getTodoItem(todoItemId);

            expect(todoItemResponse.status).toBe(404);
        });
    });

    describe("When creating a todo item", () => {
        //TODO: Missing validation for empty GUIDs
        
        test("Then the client should receive a status code of 400 when the description is empty.", async() => {

            let todoItemDescriptionResponse: AxiosResponse<TodoItem>;

            todoItemDescriptionResponse = await todoApiInterface.postTodoItem({
                id: uuidv4()  ,
                description: '',
                isCompleted: false
            });

            expect(todoItemDescriptionResponse.status).toBe(400);
        });
        
        test("Then the client should receive a status code of 201 when the body is valid JSON.", async() => {

            let todoItemCreateResponse: AxiosResponse<TodoItem>;

            todoItemCreateResponse = await todoApiInterface.postTodoItem({
                id: '666f86b8-f225-4c11-9267-45416479c334'  ,
                description: 'create todo item.',
                isCompleted: false
            });
           
            expect(todoItemCreateResponse.status).toBe(201);

            let todoItemCreateResult: TodoItem = todoItemCreateResponse.data;

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemCreateResult).toSatisfySchemaInApiSpec("TodoItem");
        });

        test("Then the client should receive a status code of 200 when retrieving the created todo item.", async() => {

            let todoItemRetrieveResponse: AxiosResponse<TodoItem>;
            
            todoItemRetrieveResponse = await todoApiInterface.getTodoItem('666f86b8-f225-4c11-9267-45416479c334');

            expect(todoItemRetrieveResponse.status).toBe(200);

            let todoItemRetrieveResult: TodoItem = todoItemRetrieveResponse.data;

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemRetrieveResult).toSatisfySchemaInApiSpec("TodoItem");
        });

        test("Then the client should receive a status code of 422 when creating a todo item with a duplicate description.", async() => {

            let todoItemDuplicateResponse: AxiosResponse<TodoItem>;

            todoItemDuplicateResponse = await todoApiInterface.postTodoItem({
                id: 'f21c0b60-c60f-49d4-94e6-9d5d3f7070d1',
                description: 'create todo item.',
                isCompleted: false
            });

            expect(todoItemDuplicateResponse.status).toBe(422);
        });
    });

    describe("When updating a todo item",  () => {
        //TODO: Missing validation for empty GUIDs

        test("Then the client should receive a status code of 201 when the body is valid JSON.", async () => {
            let todoItemCreateResponse: AxiosResponse<TodoItem>;
            todoItemCreateResponse = await todoApiInterface.postTodoItem({
                id: 'bdc01204-aed8-4241-9c64-c177901b1976',
                description: "update todo item.",
                isCompleted: false
            });

            expect(todoItemCreateResponse.status).toBe(201);

            let todoItemCreateResult: TodoItem = todoItemCreateResponse.data;

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemCreateResult).toSatisfySchemaInApiSpec("TodoItem");
        });

        test("Then the client should receive a 422 when the {id} in the path does not match the todo item id.", async () => {
            let todoItemUpdateResponse: AxiosResponse<void>;
            todoItemUpdateResponse = await todoApiInterface.putTodoItem('6623ed31-cff5-423f-a859-66739310409a', {
                id: 'bdc01204-aed8-4241-9c64-c177901b1976',
                description: "update todo item.",
                isCompleted: false
            });

            expect(todoItemUpdateResponse.status).toBe(422);
        });

        test("Then the client should receive a 204 when the {id} in the path matches the todo item id.", async () => {
            let todoItemUpdateResponse: AxiosResponse<void>;
            todoItemUpdateResponse = await todoApiInterface.putTodoItem('bdc01204-aed8-4241-9c64-c177901b1976', {
                id: 'bdc01204-aed8-4241-9c64-c177901b1976',
                description: "update todo item to something else.",
                isCompleted: false
            });

            expect(todoItemUpdateResponse.status).toBe(204);
        });
    });
});