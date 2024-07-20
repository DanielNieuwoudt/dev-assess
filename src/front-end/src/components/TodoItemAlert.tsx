import React, { FC, useState } from 'react';
import { Accordion, Alert, Button, Modal, Table } from 'react-bootstrap';
import { useTodoContext } from '../contexts/TodoContext';
import TodoItemStatus from '../enumerations/TodoItemStatus';

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
            { error && (
            <Modal show={showModal} onHide={handleCloseModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Problem Details</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <h6>{error.Title}</h6>
                    <ul>
                        {Object.entries(error.Errors).map(([field, messages]) => (
                            <li key={field}>
                                <strong>{field}:</strong>
                                <ul>
                                    {messages.map((message, index) => (
                                        <li key={index}>{message}</li>
                                    ))}
                                </ul>
                            </li>
                        ))}
                    </ul>
                    <Accordion flush>
                        <Accordion.Item eventKey="0">
                            <Accordion.Header>More information</Accordion.Header>
                            <Accordion.Body>
                                <Table striped bordered style={tableStyle}>
                                    <tr>
                                        <td>Type</td><td><a href={error.Type} target="_blank">{error.Type}</a></td>
                                    </tr>                                    
                                    <tr>
                                        <td>Status</td><td>{error.Status}</td>
                                    </tr>
                                    <tr>
                                        <td>Trace ID</td><td>{error.TraceId}</td>
                                    </tr>
                                </Table>
                            </Accordion.Body>
                        </Accordion.Item>
                    </Accordion>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseModal}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
            )}
        </>
        )
    };

    switch (status) {
        case TodoItemStatus.None:
            return getAlert("Please enter a description for your todo item.", "info", error);
        case TodoItemStatus.Added:
            return getAlert("Your todo item has been added.", "success", error);
        case TodoItemStatus.Completed:
            return getAlert("Your todo item has been marked as completed.", "success", error);
        case TodoItemStatus.Error:
            return getAlert("We encountered a problem with your todo item.", "danger", error);
        case TodoItemStatus.Refreshed:
            return getAlert("Your todo items have been refreshed.", "info", error);
    }
};

export default TodoItemAlert;