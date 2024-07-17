import { createApiClient } from '../utils/Agent'
import { TodoApi, TodoApiInterface, TodoItem } from "../generated/";
import { AxiosResponse } from "axios";
import { config } from "../config/base";
import { loadApiSpec } from "../utils/Specs";

describe("Given the backend endpoint",  () => {
    let todoApiInterface: TodoApiInterface;
    beforeEach(async () => {
        todoApiInterface = new TodoApi(undefined, config.backendBaseUrl, createApiClient());
    });

    describe("When getting todo items",  () => {
        
        test("Then the client should receive a status code of 200 when the items are returned.", async() => {
            let todoItemsResponse: AxiosResponse<TodoItem[]>;
            
            todoItemsResponse = await todoApiInterface.getTodoItems();

            expect(todoItemsResponse.status).toBe(200);

            let todoItemsResults: TodoItem[] = todoItemsResponse.data;

            loadApiSpec("webapi.openapi.yaml");
            expect(todoItemsResults).toSatisfySchemaInApiSpec("TodoItems");
        });
    });
});