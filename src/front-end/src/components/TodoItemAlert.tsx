import React, {FC} from 'react';
import { Alert, Button } from 'react-bootstrap';
import { useTodoContext } from "../contexts/TodoContext";
import TodoItemStatus from "../enumerations/TodoItemStatus";
interface TodoItemErrorProps { }

const TodoItemAlert: FC<TodoItemErrorProps> = ({ }) => {
    const { error, status, setItemStatus } = useTodoContext();
    
    switch (status){
        case TodoItemStatus.None:
            return getAlert("Please enter a description for your todo item.", "info");
        case TodoItemStatus.Added:
            return getAlert("Your todo item has been added.", "success");
        case TodoItemStatus.Completed:
            return getAlert("Your todo item has been marked as completed.", "success");
        case TodoItemStatus.Error:
            return getAlert("Your todo item has been marked as completed.", "danger");
    }
};

const getAlert = (heading: string, variant: string) => {
    return(
    <Alert variant={variant}>
        {heading}
    </Alert>);
};

export default TodoItemAlert;