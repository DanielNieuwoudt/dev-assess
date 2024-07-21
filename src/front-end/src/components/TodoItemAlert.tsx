import React, { FC, useState } from 'react';
import { Alert, Button  } from 'react-bootstrap';
import { useTodoContext } from '../contexts/TodoContext';
import TodoItemStatus from '../enumerations/TodoItemStatus';
import TodoItemStatusMessages from "../constants/TodoItemStatusMessages";
import TodoItemAlertDetails from "./TodoItemAlertDetails";

interface TodoItemErrorProps { }

const TodoItemAlert: FC<TodoItemErrorProps> = ({ }) => {
    const { error, status} = useTodoContext();
    const [showModal, setShowModal] = useState(false);

    const handleShowModal = () => setShowModal(true);
    const handleCloseModal = () => setShowModal(false);

    const tableStyle = {
        fontSize: '12px'
    };
    
    const getAlert = (message: string, variant: string, error: TodoItemError | null) => {
        return(
        <>
            <Alert variant={variant}>
                {message}
                {error && (
                    <Button variant="link" onClick={handleShowModal}>
                        View Details
                    </Button>
                )}
            </Alert>
            {error && (
                <TodoItemAlertDetails
                    showModal={showModal}
                    handleCloseModal={handleCloseModal}
                    error={error}
                />
            )}
        </>
        )
    };

    switch (status) {
        case TodoItemStatus.None:
            return getAlert(TodoItemStatusMessages.None, "info", error);
        case TodoItemStatus.Added:
            return getAlert(TodoItemStatusMessages.Added, "success", error);
        case TodoItemStatus.Completed:
            return getAlert(TodoItemStatusMessages.Completed, "success", error);
        case TodoItemStatus.Error:
            return getAlert(TodoItemStatusMessages.Error, "danger", error);
        case TodoItemStatus.Refreshed:
            return getAlert(TodoItemStatusMessages.Refreshed, "info", error);
    }
};

export default TodoItemAlert;