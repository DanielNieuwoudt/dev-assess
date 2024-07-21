import axios from 'axios';
import TodoApi from './TodoApi';
import { TodoItem } from './generated';

jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Given TodoApi When called Then', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('should {GET} todo items successfully', async () => {
        const todoItems: TodoItem[] = [
            { id: '1', description: 'Test Todo 1', isCompleted: false },
            { id: '2', description: 'Test Todo 2', isCompleted: true },
        ];

        mockedAxios.request.mockResolvedValue({ data: todoItems });

        const result = await TodoApi.getTodoItems();

        expect(result)
            .toEqual(todoItems);

        expect(mockedAxios.request)
            .toHaveBeenCalledWith(
                expect.objectContaining(
                    { 
                            method: expect.stringMatching('GET'), 
                            url: expect.stringContaining('/api/TodoItems') 
                        }));
    });

    it('should {POST} a new todo item successfully', async () => {
        const newTodoItem: TodoItem = { id: '3', description: 'New Todo', isCompleted: false };

        mockedAxios.request.mockResolvedValue(
            { 
                data: newTodoItem
            });

        const result = await TodoApi.postTodoItem(newTodoItem);

        expect(result)
            .toEqual(newTodoItem);

        expect(mockedAxios.request)
            .toHaveBeenCalledWith(expect.objectContaining(
                {
                        method: expect.stringMatching('POST'),
                        url: expect.stringContaining('/api/TodoItems') 
                    }));
    });

    it('should {PUT} a todo item successfully', async () => {
        const updatedTodoItem: TodoItem = { id: '1', description: 'Updated Todo', isCompleted: true };

        mockedAxios.request.mockResolvedValue({ data: undefined });

        await TodoApi.putTodoItem(updatedTodoItem.id!, updatedTodoItem);

        expect(mockedAxios.request)
            .toHaveBeenCalledWith(expect.objectContaining(
                {
                        method: expect.stringMatching('PUT'),
                        url: expect.stringContaining(`/api/TodoItems/${updatedTodoItem.id}`),
                        data: expect.stringMatching(JSON.stringify(updatedTodoItem))   
                    }));
    });

    it('should get backend URL correctly from environment variables', () => {
        (window as any).env = { REACT_APP_BACKEND_BASE_URL: 'https://custom-backend-url.com/api' };

        const url = TodoApi.getBackendUrl();

        expect(url)
            .toBe('https://custom-backend-url.com/api');
    });

    it('should fallback to default backend URL if environment variables are not set', () => {
        (window as any).env = undefined;

        const url = TodoApi.getBackendUrl();

        expect(url)
            .toBe('https://localhost:5001/api');
    });
});
