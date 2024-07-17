import {TodoItem} from "../generated";

export function createNewTodo(id: string, description: string): TodoItem {
    return {
        id: id,
        description: description,
        isCompleted: false
    };
}